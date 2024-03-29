namespace NeerCore.Logging.Settings;

public class TargetsSettings
{
    /// <summary>
    ///   NLog internal logger.
    /// </summary>
    public LoggerTargetSettings NLogInternal { get; set; } = new() { Enabled = false, FilePath = "./logs/nlog-internal.log" };

    /// <summary>
    ///   Defines a configuration for predefined console logger.
    /// </summary>
    public LoggerTargetSettings Console { get; set; } = new() { ShortLoggerNames = true };

    /// <summary>
    ///   Defines a configuration for predefined file logger for all log levels.
    /// </summary>
    public LoggerTargetSettings FullFile { get; set; } = new() { Enabled = false, FilePath = "${shortdate}.log" };

    /// <summary>
    ///   Defines a configuration for predefined file logger for warnings and errors.
    /// </summary>
    public LoggerTargetSettings ErrorFile { get; set; } = new() { Enabled = false, FilePath = "${shortdate}-error.log" };

    /// <summary>
    ///   Defines a configuration for predefined JSON file logger for all log levels.
    /// </summary>
    public LoggerTargetSettings JsonFile { get; set; } = new() { Enabled = false, FilePath = "${shortdate}.json" };

    /// <summary>
    ///   Defines a configuration for database target logging.
    /// </summary>
    public DatabaseTargetSettings Database { get; set; } = new() { Enabled = false };
}