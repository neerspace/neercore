using System.Collections;
using System.Globalization;
using System.Text.Json;
using NeerCore.Exceptions;

namespace NeerCore.Localization;

/// <summary>
///
/// </summary>
public readonly record struct LocalizedValue(string Language, string Value);

/// <summary>Data class that represents a localizable string.</summary>
public readonly struct LocalizedString : IEnumerable<LocalizedValue>, IEquatable<LocalizedString>
{
    /// <summary>
    ///   <see cref="LocalizedString"/> instance without stored values.
    /// </summary>
    public static readonly LocalizedString Empty = "{}";

    private readonly IDictionary<string, string> _localizations = new Dictionary<string, string>();

    /// <summary>
    ///
    /// </summary>
    /// <param name="rawSource"></param>
    public LocalizedString(string? rawSource)
    {
        if (!string.IsNullOrEmpty(rawSource) && rawSource[0] == '{')
        {
            try
            {
                _localizations = JsonSerializer.Deserialize<Dictionary<string, string>>(rawSource)!;
                return;
            }
            catch (JsonException)
            {
                // ignore
            }
        }

        _localizations.Add(CultureInfo.CurrentCulture.TwoLetterISOLanguageName, rawSource ?? string.Empty);
    }

    public LocalizedString(IDictionary<string, string> localizations)
    {
        _localizations = localizations;
    }


    /// <summary>
    ///   Total count of stored localizations.
    /// </summary>
    public int Count => _localizations.Count;

    /// <summary></summary>
    /// <param name="localizedValue"></param>
    /// <returns></returns>
    public bool Contains(string localizedValue) => _localizations.Any(loc => loc.Value == localizedValue);

    /// <summary>Determines whether the <see cref="LocalizedString"/> includes an localization with the specified <paramref name="languageCode"/>.</summary>
    /// <param name="languageCode">Two letter ISO language code.</param>
    /// <returns>True if includes, otherwise false.</returns>
    public bool ContainsLocalization(string languageCode) => _localizations.ContainsKey(languageCode);

    /// <summary></summary>
    /// <param name="languageCode"></param>
    /// <returns></returns>
    /// <exception cref="ValidationFailedException"></exception>
    public string GetLocalization(string languageCode)
    {
        if (_localizations.TryGetValue(languageCode, out string? localizedValue))
            return localizedValue;

        throw new ValidationFailedException($"Localization '{languageCode}' not provided.");
    }

    /// <summary></summary>
    /// <returns></returns>
    public string GetCurrentLocalization()
    {
        return GetLocalization(CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
    }

    /// <summary></summary>
    /// <param name="culture"></param>
    /// <returns></returns>
    public string GetLocalization(CultureInfo culture)
    {
        return GetLocalization(culture.TwoLetterISOLanguageName);
    }

    /// <summary></summary>
    /// <param name="languageCode"></param>
    /// <param name="value"></param>
    /// <exception cref="InternalServerException"></exception>
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

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public IEnumerator<LocalizedValue> GetEnumerator() =>
        _localizations.Select(l => new LocalizedValue(l.Key, l.Value)).GetEnumerator();

    /// <summary>
    ///
    /// </summary>
    public static implicit operator LocalizedString(string? value) => new(value);

    /// <summary>
    ///
    /// </summary>
    public static implicit operator string(LocalizedString localizedValue) => localizedValue.ToString();

    /// <summary>
    ///
    /// </summary>
    public static bool operator !=(LocalizedString a, LocalizedString b) => !(a == b);

    /// <summary>
    ///
    /// </summary>
    public static bool operator ==(LocalizedString a, LocalizedString b)
    {
        return a._localizations.Count == b._localizations.Count
            && !a._localizations.Except(b._localizations).Any();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool AnyLocalizationEquals(string other) => _localizations.Any(l => string.Equals(l.Value, other));

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <param name="comparisonType"></param>
    /// <returns></returns>
    public bool AnyLocalizationEquals(string other, StringComparison comparisonType) =>
        _localizations.Any(l => string.Equals(l.Value, other, comparisonType));

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(LocalizedString other) => _localizations.Equals(other._localizations);

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => obj is LocalizedString other && Equals(other);

    /// <summary>
    ///   Gets hash code.
    /// </summary>
    public override int GetHashCode() => _localizations.GetHashCode();

    /// <summary>
    ///   Converts localization to JSON string.
    /// </summary>
    public override string ToString() => JsonSerializer.Serialize(_localizations);
}