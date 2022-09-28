namespace NeerCore.Logging.Exceptions;

public sealed class InvalidLogLevelException : Exception
{
    public InvalidLogLevelException(string logLevel)
        : base($"Log level {logLevel} is not valid. " +
               "Read more about valid logging: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging") { }
}