namespace NeerCore.Logging;

/// <summary>
///   Configuration for custom <b>NeerCore</b> logger.
/// </summary>
public sealed class LoggingSettings
{
    /// <summary>
    ///   Defines maximum logging level for loggers by specified rule.
    /// </summary>
    /// <remarks>
    ///   Use '*' character in logger name as dynamic postfix.
    /// </remarks>
    /// <example>
    ///   "Microsoft.EntityFrameworkCore.*": "Warning"
    /// </example>
    public Dictionary<string, string> LogLevel { get; set; } = new();

    /// <summary>
    ///   Shared settings for all loggers.
    /// </summary>
    public SharedLoggerSettings Shared { get; set; } = new();

    /// <summary>
    ///   Configuration for any predefined logging target.
    /// </summary>
    public TargetsSettings Targets { get; set; } = new();
}

public sealed class TargetsSettings
{
    /// <summary>
    ///   Defines a configuration for predefined console logger.
    /// </summary>
    public LoggerTargetSettings Console { get; set; } = new() { ShortLoggerNames = true };

    /// <summary>
    ///   Defines a configuration for predefined file logger for all log levels.
    /// </summary>
    public LoggerTargetSettings ErrorFile { get; set; } = new() { FilePath = "${shortdate}.log" };

    /// <summary>
    ///   Defines a configuration for predefined file logger for warnings and errors.
    /// </summary>
    public LoggerTargetSettings FullFile { get; set; } = new() { FilePath = "${shortdate}-error.log" };

    /// <summary>
    ///   Defines a configuration for predefined JSON file logger for all log levels.
    /// </summary>
    public LoggerTargetSettings JsonFile { get; set; } = new() { Enabled = false, FilePath = "${shortdate}.json" };
}

public sealed class SharedLoggerSettings
{
    /// <summary>
    ///   Base directory for log files.
    /// </summary>
    public string LogsDirectoryPath { get; set; } = "~/logs/";
}

public sealed class LoggerTargetSettings
{
    /// <summary>
    ///   Disables specific logger if <b>false</b> or enables if <b>true</b>
    ///   (<b>true</b> by default).
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    ///   Path and file name of the logs output file.
    /// </summary>
    /// <remarks>
    ///   Effects only on file loggers.
    /// </remarks>
    public string? FilePath { get; set; }

    /// <summary>
    ///   If <b>true</b> – logger name in logs will be shorten
    ///   otherwise full logger names will be displayed.
    /// </summary>
    public bool ShortLoggerNames { get; set; }
}