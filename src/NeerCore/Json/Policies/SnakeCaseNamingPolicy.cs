using System.Text.Json;
using NeerCore.Extensions;

namespace NeerCore.Json.Policies;

/// <summary>
///   A snake_case_naming policy for <see cref="System.Text.Json">System.Text.Json</see>.
/// </summary>
public sealed class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    /// <summary>
    ///   Default instance.
    /// </summary>
    public static SnakeCaseNamingPolicy Instance { get; } = new();

    /// <inheritdoc cref="JsonNamingPolicy.ConvertName"/>
    public override string ConvertName(string name) => name.ToSnakeCase();
}