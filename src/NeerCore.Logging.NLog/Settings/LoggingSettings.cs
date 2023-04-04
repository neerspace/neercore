using NeerCore.Logging.Infrastructure;

namespace NeerCore.Logging.Settings;

/// <summary>
///   Configuration for custom <b>NeerCore</b> logger.
/// </summary>
public class LoggingSettings
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

    public IList<TargetBuilderBase> TargetBuilders { get; set; } = new List<TargetBuilderBase>
    {
        new ConsoleTargetBuilder(),
        new FullFileTargetBuilder(),
        new ErrorFileTargetBuilder(),
        new JsonFileTargetBuilder(),
        new DatabaseTargetBuilder(),
    };
}