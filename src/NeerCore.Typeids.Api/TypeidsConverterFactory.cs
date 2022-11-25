using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using NeerCore.Typeids.Data.EntityFramework;
using NeerCore.Typeids.Data.EntityFramework.Abstractions;
using NeerCore.DependencyInjection.Extensions;

namespace NeerCore.Typeids.Api;

public class CustomIdsConverterFactory : JsonConverterFactory
{
    private static readonly Type CustomIdBaseType = typeof(ITypeIdentifier<>);
    private static readonly Type ConverterGenericType = typeof(TypeidsJsonConverter<,>);
    private static readonly ConcurrentDictionary<Type, JsonConverter> CachedConverters = new();

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.InheritsFrom(CustomIdBaseType);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return CachedConverters.GetOrAdd(typeToConvert, key =>
        {
            var converterType = ConverterGenericType.MakeGenericType(key, key.GetIdentifierValueType());
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        });
    }
}