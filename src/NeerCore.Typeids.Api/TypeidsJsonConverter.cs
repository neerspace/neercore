using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Typeids.Abstractions;
using NeerCore.Typeids.Api.Extensions;
using NeerCore.Typeids.Internal;

namespace NeerCore.Typeids.Api;

public class TypeidsJsonConverter<TIdentifier, TValue> : JsonConverter<TIdentifier?>
    where TIdentifier : ITypeIdentifier<TValue>
{
    public override TIdentifier? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
            return GetTypeidsProcessor(options).DeserializeIdentifier<TIdentifier, TValue>(reader.GetString());

        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException();

        throw new JsonException("Element is decorated with HashidsJsonConverter \nbut is reading a non hashed id. "
            + "To allow deserialize numbers set AcceptNonHashedIds to true.");
    }

    public override void Write(Utf8JsonWriter writer, TIdentifier? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteStringValue("");
            return;
        }

        var serializedValue = GetTypeidsProcessor(options).SerializeString<TIdentifier, TValue>(value);
        writer.WriteStringValue(serializedValue);
    }


    protected static TIdentifier CreateIdentifier<T>(T value) => IdentifierFactory<TIdentifier, TValue>.CreateUnsafe(value);

    protected static ITypeidsProcessor GetTypeidsProcessor(JsonSerializerOptions options) =>
        options.GetServiceProvider().GetRequiredService<ITypeidsProcessor>();
}