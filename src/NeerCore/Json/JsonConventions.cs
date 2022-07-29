using System.Text.Json;
using System.Text.Json.Serialization;
using NeerCore.Json.Policies;

namespace NeerCore.Json;

/// <summary>
///   Provides System.Text.Json custom options presets.
/// </summary>
public static class JsonConventions
{
    /// <summary>
    ///   System.Text.Json camelCase naming policy.
    /// </summary>
    public static JsonSerializerOptions CamelCase { get; set; } = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    ///   System.Text.Json snake_case naming policy.
    /// </summary>
    public static JsonSerializerOptions SnakeCase { get; set; } = new()
    {
        PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance
    };

    /// <summary>
    ///   System.Text.Json allows JSON5 syntax features (trailing commas and comments).
    /// </summary>
    public static JsonSerializerOptions ExtendedScheme { get; set; } = new()
    {
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };
}