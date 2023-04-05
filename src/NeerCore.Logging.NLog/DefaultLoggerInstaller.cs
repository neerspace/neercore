using System.Reflection;
using NeerCore.Logging.Settings;
using NLog;
using NLog.Common;
using NLog.Config;

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

        foreach (var targetBuilder in s_settings.TargetBuilders)
        {
            targetBuilder.Settings = s_settings;
            if (targetBuilder.Enabled)
            {
                var target = targetBuilder.Build();
                targetBuilder.Configure(configuration, target);
            }
        }

        LogManager.Configuration = configuration;
    }
}