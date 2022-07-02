namespace NeerCore.Exceptions;

public class InvlidLanguageCodeException : ArgumentException
{
    public InvlidLanguageCodeException(string? languageCode)
        : base($"Invalid language code provided: '{languageCode}'") { }
}