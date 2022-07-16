using System.Text.RegularExpressions;

namespace NeerCore.Extensions;

public static class StringExtensions
{
    /// <summary>
    ///   Returns a source string <paramref name="value"/> if it's length
    ///   is less then <paramref name="limit"/> or substring with length
    ///   equals the <paramref name="limit"/> and starts at the index 0.
    /// </summary>
    /// <param name="value">Source not null string.</param>
    /// <param name="limit">Max length for the <paramref name="value"/>.</param>
    /// <returns>Limited string.</returns>
    public static string Limited(this string value, int limit)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
        return value.Length > limit ? value[..limit] : value;
    }

    /// <summary>Converts 'MyExampleString' to 'My example string'.</summary>
    /// <param name="value">Source pascal or camel case 'MyExampleString'.</param>
    /// <returns>Separate words string 'My example string'.</returns>
    public static string CamelCaseToWords(this string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));

        string result = Regex.Replace(value, "(\\B[A-Z])", " $1");
        return result[0].ToString().ToUpperInvariant() + result[1..].ToLowerInvariant();
    }

    /// <summary>Converts 'MyExampleString' to 'my_example_string'.</summary>
    /// <param name="value">Source pascal or camel case 'MyExampleString'.</param>
    /// <returns>Camel case string 'my_example_string'.</returns>
    public static string ToSnakeCase(this string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));

        return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
    }

    /// <summary>
    ///   <b>Wrapper for built-in method <see cref="System.String.IsNullOrEmpty"/>.</b>
    ///   <br/>
    ///   Indicates whether the specified string is null or an empty string ("").
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
    public static bool IsNullOrEmpty(this string? value) => string.IsNullOrEmpty(value);

    /// <summary>
    ///   <b>Wrapper for built-in method <see cref="System.String.IsNullOrWhiteSpace"/>.</b>
    ///   <br/>
    ///   Indicates whether the specified string is null, white space (" ") or an empty string ("").
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns>true if the value parameter is null or Empty, or if value consists exclusively of white-space characters.</returns>
    public static bool IsNullOrWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value);
}