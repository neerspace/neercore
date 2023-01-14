namespace NeerCore.Localization.Exceptions;

/// <summary>
///   This exception will be thrown if you try to get localization
///   for locale that does not provided in the current localized value.
/// </summary>
public sealed class InvalidLanguageCodeException : ArgumentException
{
    /// <param name="languageCode">Two letter language ISO code.</param>
    public InvalidLanguageCodeException(string? languageCode)
        : base($"Invalid language code provided: '{languageCode}'") { }
}