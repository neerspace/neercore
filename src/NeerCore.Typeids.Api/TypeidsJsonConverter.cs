using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Typeids.Api.Extensions;
using NeerCore.Typeids.Data.EntityFramework;
using NeerCore.Typeids.Data.EntityFramework.Abstractions;

namespace NeerCore.Typeids.Api;

public class TypeidsJsonConverter<TIdentifier, TValue> : JsonConverter<TIdentifier>
    where TIdentifier : ITypeIdentifier<TValue>
    where TValue : new()
{
    public override TIdentifier Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
            return GetCustomIdsProcessor(options).DeserializeIdentifier<TIdentifier, TValue>(reader.GetString());

        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException();

        throw new JsonException("Element is decorated with HashidsJsonConverter \nbut is reading a non hashed id. " +
                                "To allow deserialize numbers set AcceptNonHashedIds to true.");
    }

    public override void Write(Utf8JsonWriter writer, TIdentifier value, JsonSerializerOptions options)
    {
        string? serializedValue = GetCustomIdsProcessor(options).SerializeString<TIdentifier, TValue>(value);
        writer.WriteStringValue(serializedValue);
    }


    protected static TIdentifier CreateIdentifier<T>(T value) => IdentifierFactory<TIdentifier, TValue>.CreateUnsafe(value);

    protected static ITypeidsProcessor GetCustomIdsProcessor(JsonSerializerOptions options) => options.GetServiceProvider().GetRequiredService<ITypeidsProcessor>();
}