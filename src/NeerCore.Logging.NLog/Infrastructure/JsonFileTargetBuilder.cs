using NLog.Layouts;
using NLog.Targets;

namespace NeerCore.Logging.Infrastructure;

public class JsonFileTargetBuilder : FileTargetBuilderBase
{
    public override bool Enabled => Settings.Targets.JsonFile.Enabled;

    protected virtual JsonLayout JsonLayout { get; set; } = new()
    {
        Attributes =
        {
            new JsonAttribute("datetime", "${longdate}"),
            new JsonAttribute("level", "${level:uppercase=true}"),
            new JsonAttribute("logger", "${logger}"),
            new JsonAttribute("message", "${message}"),
            new JsonAttribute("exception", "${exception:format=ToString}"),
        }
    };

    public override Target Build()
    {
        var targetSettings = Settings.Targets.JsonFile;

        var layout = JsonLayout;
        if (targetSettings.ShortLoggerNames)
        {
            var loggerAttr = layout.Attributes.FirstOrDefault(a => a.Name.EndsWith("{logger}"));
            if (loggerAttr is not null)
                loggerAttr.Layout = "${logger:shortname=true}";
        }

        return new FileTarget("logJsonFile")
        {
            FileName = BuildLogFilePath(targetSettings.FilePath),
            Layout = layout,
        };
    }
}