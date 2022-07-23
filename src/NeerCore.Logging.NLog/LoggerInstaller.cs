using System.Reflection;
using Microsoft.Extensions.Configuration;
using NeerCore.DependencyInjection.Extensions;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace NeerCore.Logging;

/// <summary>
///   NLog logger installer. Configures and creates logger.
/// </summary>
public static class LoggerInstaller
{
    public const string AppSettingsName = "appsettings.json";

    public static ILogger InitDefault(Action<LoggingSettings>? configureOptions = null, string? defaultLoggerName = null)
    {
        DefaultLoggerInstaller.Configure(configureOptions);
        return CreateDefaultLogger(defaultLoggerName);
    }

    public static ILogger InitFromCurrentEnvironment(string sectionName = "Logging", string environmentVariableName = "ASPNETCORE_ENVIRONMENT", string? defaultLoggerName = null)
    {
        return InitFromEnvironment(Environment.GetEnvironmentVariable(environmentVariableName), sectionName, defaultLoggerName);
    }

    public static ILogger InitFromEnvironment(string? environment, string sectionName = "Logging", string? defaultLoggerName = null)
    {
        string configurationPath = string.IsNullOrEmpty(environment) ? AppSettingsName : $"appsettings.{environment}.json";
        var loggingConfigSection = BuildConfiguration(configurationPath, AppSettingsName).GetSection(sectionName);
        DefaultLoggerInstaller.Configure(loggingConfigSection.Bind);
        return CreateDefaultLogger(defaultLoggerName);
    }

    public static ILogger InitFromFile(string configurationPath = "NLog.json", string rootSectionName = "NLog", string? defaultLoggerName = null)
    {
        var config = BuildConfiguration(configurationPath);

        LogManager.Configuration = new NLogLoggingConfiguration(config.GetRequiredSection(rootSectionName));
        return NLogBuilder.ConfigureNLog(LogManager.Configuration).GetLogger(defaultLoggerName);
    }

    private static ILogger CreateDefaultLogger(string? loggerName)
    {
        loggerName ??= Assembly.GetEntryAssembly()!.GetBaseNamespace() + ".Program";
        return NLogBuilder.ConfigureNLog(LogManager.Configuration).GetLogger(loggerName);
    }

    private static IConfigurationRoot BuildConfiguration(string configurationPath, string? altConfigurationPath = null)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configurationPath, optional: true);
        if (!string.IsNullOrEmpty(altConfigurationPath))
            configurationBuilder = configurationBuilder
                .AddJsonFile(altConfigurationPath, optional: false);

        return configurationBuilder.Build();
    }
}