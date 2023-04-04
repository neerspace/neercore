using System.Reflection;
using NeerCore.Logging.Settings;
using NLog;
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

        var configuration = new LoggingConfiguration();

        foreach (var targetBuilder in s_settings.TargetBuilders)
        {
            targetBuilder.Settings = s_settings;
            if (targetBuilder.Enabled)
            {
                var target = targetBuilder.Build();
                targetBuilder.Configure(configuration, target);
            }
        }

        ConfigurationItemFactory.Default.RegisterItemsFromAssembly(Assembly.Load("NLog.Extensions.Logging"));
        ConfigurationItemFactory.Default.RegisterItemsFromAssembly(Assembly.Load("NLog.Web.AspNetCore"));
        LogManager.Configuration = configuration;
    }
}