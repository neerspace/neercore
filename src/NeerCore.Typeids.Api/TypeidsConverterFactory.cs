using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using NeerCore.DependencyInjection.Extensions;
using NeerCore.Typeids.Abstractions;
using NeerCore.Typeids.Internal;

namespace NeerCore.Typeids.Api;

public class TypeidsConverterFactory : JsonConverterFactory
{
    private static readonly Type customIdBaseType = typeof(ITypeIdentifier<>);
    private static readonly Type converterGenericType = typeof(TypeidsJsonConverter<,>);
    private static readonly ConcurrentDictionary<Type, JsonConverter> cachedConverters = new();

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.InheritsFrom(customIdBaseType);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return cachedConverters.GetOrAdd(typeToConvert, key =>
        {
            var converterType = converterGenericType.MakeGenericType(key, key.GetIdentifierValueType());
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        });
    }
}