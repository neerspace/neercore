namespace NeerCore.Data.Exceptions;

public sealed class InvalidLanguageCodeException : ArgumentException
{
    public InvalidLanguageCodeException(string? languageCode)
            : base($"Invalid language code provided: '{languageCode}'") { }
}