using NLog;
using NLog.Config;
using NLog.Targets;

namespace NeerCore.Logging.Infrastructure;

public class ErrorFileTargetBuilder : FileTargetBuilderBase
{
    public override bool Enabled => Settings.Targets.FullFile.Enabled;


    public override Target Build()
    {
        var targetSettings = Settings.Targets.FullFile;

        return new FileTarget("logErrorsFile")
        {
            FileName = BuildLogFilePath(targetSettings.FilePath),
            Layout = FormatLayout(FileLayout, targetSettings)
        };
    }

    public override void Configure(LoggingConfiguration configuration, Target target)
    {
        configuration.AddTarget(target.Name, target);
        configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Warn, target));
    }
}