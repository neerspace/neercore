using System.Reflection;
using NeerCore.Logging.Exceptions;
using NeerCore.Logging.Settings;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace NeerCore.Logging;

internal static class DefaultLoggerInstaller
{
    private static readonly LoggingSettings s_settings = new();


    internal static void Configure(Action<LoggingSettings>? configureOptions = null)
    {
        configureOptions?.Invoke(s_settings);
        if (!s_settings.LogLevel.ContainsKey("*"))
            s_settings.LogLevel.Add("*", "Warning");

        ConfigurationItemFactory.Default.RegisterItemsFromAssembly(Assembly.Load("NLog.Extensions.Logging"));
        ConfigurationItemFactory.Default.RegisterItemsFromAssembly(Assembly.Load("NLog.Web.AspNetCore"));

        var configuration = new LoggingConfiguration();

        if (s_settings.Targets.NLogInternal.Enabled)
        {
            InternalLogger.LogLevel = LogLevel.Warn;
            InternalLogger.LogFile = s_settings.Targets.NLogInternal.FilePath;
        }
        else
        {
            InternalLogger.LogLevel = LogLevel.Off;
        }

        var sharedTarget = new SplitGroupTarget("neerTarget");
        var sharedTargetUsed = false;
        foreach (var targetBuilder in s_settings.TargetBuilders)
        {
            targetBuilder.Settings = s_settings;
            if (!targetBuilder.Enabled)
                continue;

            var target = targetBuilder.Build();
            if (targetBuilder.UseAsSeparateTarget)
            {
                targetBuilder.Configure(configuration, target);
                configuration.AddTarget(target);
            }
            else
            {
                sharedTargetUsed = true;
                sharedTarget.Targets.Add(target);
            }
        }

        if (sharedTargetUsed)
        {
            configuration.AddTarget(sharedTarget);
            configuration.ApplyLogLevelsFromSettings(sharedTarget);
        }

        LogManager.Configuration = configuration;
    }


    private static void ApplyLogLevelsFromSettings(this LoggingConfiguration configuration, Target target)
    {
        foreach (var logLevel in s_settings.LogLevel)
        {
            bool isFinal = logLevel.Key != "*";
            configuration.AddRule(ParseLogLevel(logLevel.Value), LogLevel.Fatal, target, logLevel.Key, final: isFinal);
        }
    }

    private static LogLevel ParseLogLevel(string logLevelValue) => logLevelValue switch
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
}