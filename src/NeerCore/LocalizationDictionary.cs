using System.Collections;
using System.Diagnostics.CodeAnalysis;
using NeerCore.Exceptions;

namespace NeerCore;

public class LocalizationDictionary : IDictionary<string, string>
{
    private readonly Dictionary<string, string> _dict;

    public LocalizationDictionary(Dictionary<string, string> dict)
    {
        _dict = dict;
    }

    public string this[string languageCode]
    {
        get
        {
            if (IsValidLanguageCode(languageCode))
                return _dict[languageCode];
            throw new InvlidLanguageCodeException(languageCode);
        }
        set
        {
            if (_dict.ContainsKey(languageCode))
                _dict[languageCode] = value;
            else
                Add(languageCode, value);
        }
    }

    public ICollection<string> Keys => _dict.Keys;
    public ICollection<string> Values => _dict.Values;
    public int Count => _dict.Count;
    public bool IsReadOnly => false;

    public void Add(string languageCode, string localizedValue)
    {
        if (IsValidLanguageCode(languageCode))
            _dict.Add(languageCode, localizedValue);
        else
            throw new InvlidLanguageCodeException(languageCode);
    }

    public void Add(KeyValuePair<string, string> localization) => Add(localization.Key, localization.Value);
    public void Clear() => _dict.Clear();
    public bool Contains(KeyValuePair<string, string> localization) => _dict.Contains(localization);
    public bool ContainsKey(string languageCode) => IsValidLanguageCode(languageCode) && _dict.ContainsKey(languageCode);

    public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
    {
        if (array.Length - arrayIndex < _dict.Count)
            throw new ArgumentException($"Not enough length of the array to copy there. (Array length is {array.Length - arrayIndex} at index {arrayIndex})");

        foreach (var localization in _dict)
        {
            array[arrayIndex] = localization;
            arrayIndex++;
        }
    }


    IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _dict.GetEnumerator();

    public bool Remove(string languageCode) => _dict.Remove(languageCode);
    public bool Remove(KeyValuePair<string, string> localization) => _dict.Remove(localization.Key);

    public bool TryGetValue(string languageCode, [MaybeNullWhen(false)] out string localization)
    {
        if (IsValidLanguageCode(languageCode))
            return _dict.TryGetValue(languageCode, out localization);
        localization = null;
        return false;
    }

    private static bool IsValidLanguageCode(string languageCode) => languageCode is { Length: 2 };
}