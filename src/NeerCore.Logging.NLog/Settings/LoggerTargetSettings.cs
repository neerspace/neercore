namespace NeerCore.Logging.Settings;

public class LoggerTargetSettings
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