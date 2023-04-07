using NeerCore.Logging.Settings;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace NeerCore.Logging;

public abstract class TargetBuilderBase
{
    public LoggingSettings Settings { get; set; }

    public abstract bool Enabled { get; }
    public abstract Target Build();

    public virtual bool UseAsSeparateTarget { get; }
    public virtual void Configure(LoggingConfiguration configuration, Target target) { }


    protected Layout FormatLayout(string layout, LoggerTargetSettings loggerTargetSettings)
    {
        return loggerTargetSettings.ShortLoggerNames
            ? layout.Replace("${logger}", "${logger:shortname=true:padding=20}")
            : layout.Replace("${logger}", "${logger:padding=30}");
    }
}