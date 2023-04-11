using System.Text.Json;
using AspNetCore.Hashids.Options;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Typeids.Abstractions;

namespace NeerCore.Typeids.Api.Hashids;

public class TypeidsHashidsJsonConverter<TIdentifier, TValue> : TypeidsJsonConverter<TIdentifier, TValue>
    where TIdentifier : ITypeIdentifier<TValue>
    where TValue : new()
{
    public override TIdentifier Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
            return GetTypeidsProcessor(options).DeserializeIdentifier<TIdentifier, TValue>(reader.GetString())!;

        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException();

        if (GetHashidsOptions(options).AcceptNonHashedIds)
            return CreateIdentifier(reader.GetInt32());

        throw new JsonException("Element is decorated with HashidsJsonConverter \nbut is reading a non hashed id."
            + "To allow deserialize numbers set AcceptNonHashedIds to true.");
    }


    public override void Write(Utf8JsonWriter writer, TIdentifier value, JsonSerializerOptions options)
    {
        var idsProcessor = GetTypeidsProcessor(options);
        string? serializedValue = idsProcessor.SerializeString<TIdentifier, TValue>(value);
        writer.WriteStringValue(serializedValue);
    }

    private static HashidsOptions GetHashidsOptions(JsonSerializerOptions options) =>
        options.GetServiceProvider().GetRequiredService<HashidsOptions>();
}