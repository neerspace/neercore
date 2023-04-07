using NLog.Targets;

namespace NeerCore.Logging.Infrastructure;

public class FullFileTargetBuilder : FileTargetBuilderBase
{
    public override bool Enabled => Settings.Targets.FullFile.Enabled;


    public override Target Build()
    {
        var targetSettings = Settings.Targets.FullFile;

        return new FileTarget("logFile")
        {
            FileName = BuildLogFilePath(targetSettings.FilePath),
            Layout = FormatLayout(FileLayout, targetSettings)
        };
    }
}