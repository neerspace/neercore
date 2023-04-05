using NeerCore.Logging.Exceptions;
using NeerCore.Logging.Settings;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace NeerCore.Logging;

public abstract class TargetBuilderBase
{
    public LoggingSettings Settings { get; set; }

    public abstract bool Enabled { get; }
    public abstract Target Build();
    public abstract void Configure(LoggingConfiguration configuration, Target target);


    protected void ApplyLogLevelsFromSettings(LoggingConfiguration configuration, Target target)
    {
        foreach (var logLevel in Settings.LogLevel)
        {
            bool isFinal = logLevel.Key != "*";
            configuration.AddRule(ParseLogLevel(logLevel.Value), LogLevel.Fatal, target, logLevel.Key, final: isFinal);
        }
    }

    protected LogLevel ParseLogLevel(string logLevelValue) => logLevelValue switch
    {
        "Trace"                 => LogLevel.Trace,
        "Debug"                 => LogLevel.Debug,
        "Information" or "Info" => LogLevel.Info,
        "Warning" or "Warn"     => LogLevel.Warn,
        "Error"                 => LogLevel.Error,
        "Critical" or "Fatal"   => LogLevel.Fatal,
        "None" or "Off"         => LogLevel.Off,
        _                       => throw new InvalidLogLevelException(logLevelValue)
    };

    protected Layout FormatLayout(string layout, LoggerTargetSettings loggerTargetSettings)
    {
        return loggerTargetSettings.ShortLoggerNames
            ? layout.Replace("${logger}", "${logger:shortname=true:padding=20}")
            : layout.Replace("${logger}", "${logger:padding=30}");
    }
}