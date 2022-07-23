namespace NeerCore.Logging;

public class LoggingSettings
{
    public Dictionary<string, string> LogLevel { get; set; } = new();

    public SharedLoggerSettings Shared { get; set; } = new();
    public SingleLoggerSettings ConsoleLogger { get; set; } = new() { ShortLoggerNames = true };
    public SingleLoggerSettings FullFileLogger { get; set; } = new() { FilePath = "${shortdate}.log" };
    public SingleLoggerSettings ErrorFileLogger { get; set; } = new() { FilePath = "${shortdate}-error.log" };
}

public class SharedLoggerSettings
{
    public string LogsDirectoryPath { get; set; } = "~/logs/";
}

public class SingleLoggerSettings
{
    public bool Enabled { get; set; } = true;
    public string? FilePath { get; set; }
    public bool ShortLoggerNames { get; set; }
}