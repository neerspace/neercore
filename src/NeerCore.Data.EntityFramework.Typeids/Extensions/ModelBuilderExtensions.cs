using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NeerCore.Data.Abstractions;
using NeerCore.Data.EntityFramework.Typeids.Abstractions;
using NeerCore.DependencyInjection;
using NeerCore.DependencyInjection.Extensions;

namespace NeerCore.Data.EntityFramework.Typeids.Extensions;

public static class ModelBuilderExtensions
{
    private static readonly Type CustomIdBaseType = typeof(ITypeIdentifier<>);
    private static readonly ConcurrentDictionary<Type, ValueConverter> Converters = new();
    private static readonly Type[] NumericType = { typeof(byte), typeof(short), typeof(int), typeof(long) };


    public static void UseTypedIdsForAssembly(this ModelBuilder modelBuilder, Assembly entitiesAssembly)
    {
        foreach (var entityType in AssemblyProvider.GetImplementationsFromAssembly<IEntity>(entitiesAssembly))
        foreach (var property in entityType.GetProperties())
        {
            var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);

            if (underlyingType is not null && underlyingType.InheritsFrom(CustomIdBaseType)
                || property.PropertyType.InheritsFrom(CustomIdBaseType))
            {
                var converter = Converters.GetOrAdd(property.PropertyType,
                    identifierType =>
                    {
                        var underlyingIdType = Nullable.GetUnderlyingType(identifierType);
                        if (underlyingIdType is null)
                        {
                            var valueType = identifierType.GetIdentifierValueType();
                            return CreateCustomIdConverter(identifierType, valueType);
                        }
                        else
                        {
                            var valueType = underlyingIdType.GetIdentifierValueType();
                            return CreateNullableCustomIdConverter(identifierType, valueType);
                        }
                    });

                var propBuilder = modelBuilder.Entity(entityType)
                    .Property(property.Name)
                    .HasConversion(converter);

                if (property.Name.ToLower() == "id" && NumericType.Contains(property.PropertyType.GetProperty("Value")!.PropertyType))
                    propBuilder.ValueGeneratedOnAdd();
            }
        }
    }


    private static ValueConverter CreateCustomIdConverter(Type identifierType, Type valueType)
    {
        // id => id.Value
        var toProviderExpression = CreateToProviderExpression(identifierType, valueType);

        // value => new TIdentifier(value)
        var fromProviderExpression = CreateFromProviderExpression(identifierType, valueType);

        var converterType = typeof(ValueConverter<,>).MakeGenericType(identifierType, valueType);

        return (ValueConverter)Activator.CreateInstance(converterType, toProviderExpression, fromProviderExpression, null)!;
    }

    private static ValueConverter CreateNullableCustomIdConverter(Type identifierNullableType, Type valueType)
    {
        // id => id.HasValue ? id.Value.Value : null
        var toProviderExpression = CreateNullableToProviderExpression(identifierNullableType, valueType);

        // value => value.HasValue ? new TIdentifier(value.Value) : null,
        var fromProviderExpression = CreateNullableFromProviderExpression(identifierNullableType, valueType);

        var converterType = typeof(ValueConverter<,>).MakeGenericType(identifierNullableType, valueType);

        // new ValueConverter<ResourceId?, Guid>(
        //     id => id.HasValue ? id.Value.Value : Guid.Empty,
        //     value => new ResourceId(value),
        //     null);

        return (ValueConverter)Activator.CreateInstance(converterType, toProviderExpression, fromProviderExpression, null)!;
    }


    private static LambdaExpression CreateFromProviderExpression(Type identifierType, Type valueType)
    {
        var fromProviderFuncType = typeof(Func<,>).MakeGenericType(valueType, identifierType);
        var valueParam = Expression.Parameter(valueType, "value");
        var ctor = identifierType.GetConstructor(new[] { valueType })!;
        var fromProviderExpression = Expression.Lambda(fromProviderFuncType, Expression.New(ctor, valueParam), valueParam);
        return fromProviderExpression;
    }

    private static LambdaExpression CreateToProviderExpression(Type identifierType, Type valueType)
    {
        var toProviderFuncType = typeof(Func<,>).MakeGenericType(identifierType, valueType);
        var stronglyTypedIdParam = Expression.Parameter(identifierType, "id");
        var valueProp = Expression.Property(stronglyTypedIdParam, "Value");
        var toProviderExpression = Expression.Lambda(toProviderFuncType, valueProp, stronglyTypedIdParam);
        return toProviderExpression;
    }

    private static LambdaExpression CreateNullableFromProviderExpression(Type identifierNullableType, Type valueType)
    {
        var identifierType = Nullable.GetUnderlyingType(identifierNullableType)!;

        var fromProviderFuncType = typeof(Func<,>).MakeGenericType(valueType, identifierNullableType);
        var valueParam = Expression.Parameter(valueType, "value");
        var ctor = identifierType.GetConstructor(new[] { valueType })!;
        var ctorNew = Expression.New(ctor, valueParam);
        var castToNullable = Expression.TypeAs(ctorNew, identifierNullableType);
        var fromProviderExpression = Expression.Lambda(fromProviderFuncType, castToNullable, valueParam);
        return fromProviderExpression;
    }

    private static LambdaExpression CreateNullableToProviderExpression(Type identifierNullableType, Type valueType)
    {
        var toProviderFuncType = typeof(Func<,>).MakeGenericType(identifierNullableType, valueType);

        // id.Value.Value
        var identifierParam = Expression.Parameter(identifierNullableType, "id");
        var valueProp = Expression.Property(identifierParam, "Value");
        valueProp = Expression.Property(valueProp, "Value");

        // null
        var @default = Expression.Default(valueType);

        // id.HasValue
        var hasValue = Expression.Property(identifierParam, "HasValue");

        // id.HasValue ? id.Value.Value : null
        var condition = Expression.Condition(hasValue, valueProp, @default);

        var toProviderExpression = Expression.Lambda(toProviderFuncType, condition, identifierParam);
        return toProviderExpression;
    }
}