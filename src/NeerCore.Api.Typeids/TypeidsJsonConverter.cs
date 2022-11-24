using System.Text.Json;
using System.Text.Json.Serialization;
using NeerCore.Data.EntityFramework.Typeids;

namespace NeerCore.Api.Typeids;

public class TypeidsJsonConverter<TIdentifier, TValue> : JsonConverter<TIdentifier>
    where TIdentifier : NeerCore.Data.EntityFramework.Typeids.Abstractions.ITypeIdentifier<TValue>
    where TValue : new()
{
    public override TIdentifier Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
            return GetCustomIdsProcessor(options).DeserializeIdentifier<TIdentifier, TValue>(reader.GetString());

        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException();

        if (GetHashidsOptions(options).AcceptNonHashedIds)
            return CreateIdentifier(reader.GetInt32());

        throw new JsonException("Element is decorated with HashidsJsonConverter \nbut is reading a non hashed id. To allow deserialize numbers set AcceptNonHashedIds to true.");
    }


    public override void Write(Utf8JsonWriter writer, TIdentifier value, JsonSerializerOptions options)
    {
        string? serializedValue = GetCustomIdsProcessor(options).SerializeString<TIdentifier, TValue>(value);
        writer.WriteStringValue(serializedValue);
    }


    private static TIdentifier CreateIdentifier<T>(T value) => IdentifierFactory<TIdentifier, TValue>.CreateUnsafe(value);

    private static ITypeidsProcessor GetCustomIdsProcessor(JsonSerializerOptions options) => options.GetServiceProvider().GetRequiredService<ITypeidsProcessor>();
    private static HashidsOptions GetHashidsOptions(JsonSerializerOptions options) => options.GetServiceProvider().GetRequiredService<HashidsOptions>();
}