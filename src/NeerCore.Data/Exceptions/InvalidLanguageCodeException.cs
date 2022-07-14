namespace NeerCore.Data.Exceptions;

public class InvalidLanguageCodeException : ArgumentException
{
    public InvalidLanguageCodeException(string? languageCode)
            : base($"Invalid language code provided: '{languageCode}'") { }
}