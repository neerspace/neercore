using System.Text.Json;
using System.Text.Json.Serialization;

namespace NeerCore.Localization;

/// <summary>
///   Custom <b>System.Text.Json</b> JSON converter for <see cref="LocalizedString"/>.
/// </summary>
public class LocalizedStringJsonConverter : JsonConverter<LocalizedString>
{
    /// <summary>
    ///   Writes <see cref="LocalizedString"/> as friendly JSON.
    /// </summary>
    public override LocalizedString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject token");

        var localizations = new Dictionary<string, string>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return new LocalizedString(localizations);

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException("Expected PropertyName token");

            var propName = reader.GetString()!;
            reader.Read();
            localizations.Add(propName, reader.GetString() ?? "");
        }

        throw new JsonException("Expected EndObject token");
    }

    /// <summary>
    ///   Reads <see cref="LocalizedString"/> from friendly JSON.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, LocalizedString value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        if (value.Count > 0)
        {
            foreach (var localization in value)
            {
                writer.WritePropertyName(localization.Language);
                writer.WriteStringValue(localization.Value);
            }
        }

        writer.WriteEndObject();
    }
}