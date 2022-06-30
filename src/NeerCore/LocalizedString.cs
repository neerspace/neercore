using System.Collections;
using System.Globalization;
using System.Text.Json;
using NeerCore.Exceptions;

namespace NeerCore;

public readonly struct LocalizedString : IEnumerable<KeyValuePair<string, string>>, IEquatable<LocalizedString>
{
	private const string Default = "{}";
	public static readonly LocalizedString Empty = Default;

	private readonly IDictionary<string, string> _localizations;

	public LocalizedString(string? rawValue)
	{
		if (string.IsNullOrEmpty(rawValue)) rawValue = Default;
		_localizations = JsonSerializer.Deserialize<Dictionary<string, string>>(rawValue)!;
	}

	public bool Contains(string localizedValue) => _localizations.Any(loc => loc.Value == localizedValue);

	public bool ContainsLocalization(string languageCode) => _localizations.ContainsKey(languageCode);

	public string GetLocalization(string? languageCode = null)
	{
		languageCode ??= CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
		if (_localizations.TryGetValue(languageCode, out string? localizedValue))
			return localizedValue;

		throw new ValidationFailedException($"Localization '{languageCode}' not provided.");
	}

	public string GetLocalization(CultureInfo culture)
	{
		return GetLocalization(culture.TwoLetterISOLanguageName);
	}


	public void SetLocalization(string languageCode, string value)
	{
		if (languageCode is { Length: 2 })
			_localizations.Add(languageCode, value);

		throw new InternalServerException("Invalid localization code provided.");
	}

	public void SetLocalization(CultureInfo culture, string value)
	{
		SetLocalization(culture.TwoLetterISOLanguageName, value);
	}


	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _localizations.GetEnumerator();

	public static implicit operator LocalizedString(string? value) => new(value);
	public static implicit operator string(LocalizedString localizedValue) => localizedValue.ToString();

	public static bool operator !=(LocalizedString a, LocalizedString b) => !(a == b);

	public static bool operator ==(LocalizedString a, LocalizedString b)
	{
		return a._localizations.Count == b._localizations.Count
		       && !a._localizations.Except(b._localizations).Any();
	}

	public bool Equals(LocalizedString other) => _localizations.Equals(other._localizations);
	public override bool Equals(object? obj) => obj is LocalizedString other && Equals(other);
	public override int GetHashCode() => _localizations.GetHashCode();

	public override string ToString() => JsonSerializer.Serialize(_localizations);
}