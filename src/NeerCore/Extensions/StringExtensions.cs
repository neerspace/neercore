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
    public static string ApplyLimit(this string value, int limit)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
        return value.Length > limit ? value[..limit] : value;
    }

    /// <summary>Converts 'MyExampleString' to 'my_example_string'.</summary>
    /// <param name="value">Source pascal or camel case 'MyExampleString'.</param>
    /// <returns>Camel case string 'my_example_string'.</returns>
    public static string ToSnakeCase(this string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));

        return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
    }
}