using System.Text.RegularExpressions;

namespace NeerCore.Extensions;

public static class StringExtensions
{
	/// <summary>
	/// Converts 'CamelCaseString' to 'Camel case string'
	/// </summary>
	/// <param name="value">Source 'CamelCaseString'</param>
	/// <returns>Result 'Camel case string'</returns>
	public static string CamelCaseToWords(this string value)
	{
		if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));

		string result = Regex.Replace(value, "(\\B[A-Z])", " $1");
		return result[0] + result[1..].ToLower();
	}

	public static bool IsNullOrEmpty(this string? value) => string.IsNullOrEmpty(value);
	public static bool IsNullOrWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value);
}