using System.Reflection;
using System.Text.RegularExpressions;
using NeerCore.Logging.Exceptions;
using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace NeerCore.Logging;

internal static class DefaultLoggerInstaller
{
    private static readonly LoggingSettings s_settings = new();

    private const string DateTimeRegExp = @"\[(2[0-3]|[01]?[0-9]):([0-5]?[0-9]):([0-5]?[0-9])\.\d\d\d\d\]";

    internal static void Configure(Action<LoggingSettings>? configureOptions = null)
    {
        configureOptions?.Invoke(s_settings);
        if (!s_settings.LogLevel.ContainsKey("*"))
            s_settings.LogLevel.Add("*", "Warning");

        var configuration = new LoggingConfiguration();

        if (s_settings.Targets.Console is { Enabled: true })
        {
            ColoredConsoleTarget logConsole = CreateConsoleLogTarget(s_settings.Targets.Console);
            configuration.AddTarget(nameof(logConsole), logConsole);
            ApplyLogLevelsFromSettings(configuration, logConsole);
        }

        if (s_settings.Targets.ErrorFile is { Enabled: true })
        {
            FileTarget logErrorsFile = CreateErrorFileLogTarget(s_settings.Targets.ErrorFile);
            configuration.AddTarget(nameof(logErrorsFile), logErrorsFile);
            configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Warn, logErrorsFile));
        }

        if (s_settings.Targets.FullFile is { Enabled: true })
        {
            FileTarget logFile = CreateFullFileLogTarget(s_settings.Targets.FullFile);
            configuration.AddTarget(nameof(logFile), logFile);
            ApplyLogLevelsFromSettings(configuration, logFile);
        }

        if (s_settings.Targets.JsonFile is { Enabled: true })
        {
            FileTarget logFile = CreateFullJsonLogTarget(s_settings.Targets.JsonFile);
            configuration.AddTarget(nameof(logFile), logFile);
            ApplyLogLevelsFromSettings(configuration, logFile);
        }

        ConfigurationItemFactory.Default.RegisterItemsFromAssembly(Assembly.Load("NLog.Extensions.Logging"));
        ConfigurationItemFactory.Default.RegisterItemsFromAssembly(Assembly.Load("NLog.Web.AspNetCore"));
        LogManager.Configuration = configuration;
    }

    private static void ApplyLogLevelsFromSettings(LoggingConfiguration configuration, Target target)
    {
        // string applicationNamespace = Assembly.GetEntryAssembly()!.GetBaseNamespace();

        foreach (var logLevel in s_settings.LogLevel)
        {
            // TODO: Explore more about final rules
            bool isFinal = false; // logLevel.Key != "*" && !logLevel.Key.StartsWith(applicationNamespace);
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

    private static string BuildLogFilePath(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException(nameof(fileName), "File name is not invalid.");

        string path = s_settings.Shared.LogsDirectoryPath + fileName;

        if (path.Contains('~'))
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory + "/";
            path = path.Replace("~", basePath)
                .Replace("//", string.Empty)
                .Replace("//", string.Empty);
        }

        int iter = 100;
        const string pathRegex = @"((?!\.)[^\/]+\/)\.\.\/";
        while (path.Contains("../") && iter-- > 0)
            path = Regex.Replace(path, pathRegex, "");

        if (iter == 0)
            throw new LockRecursionException("Cannot parse path.");

        return path;
    }

    private static FileTarget CreateErrorFileLogTarget(LoggerTargetSettings targetSettings)
    {
        return new FileTarget("logErrorsFile")
        {
            FileName = BuildLogFilePath(targetSettings.FilePath),
            Layout = FormatLayout(LoggerLayouts.FileLayout, targetSettings)
        };
    }

    private static FileTarget CreateFullFileLogTarget(LoggerTargetSettings targetSettings)
    {
        return new FileTarget("logFile")
        {
            FileName = BuildLogFilePath(targetSettings.FilePath),
            Layout = FormatLayout(LoggerLayouts.FileLayout, targetSettings)
        };
    }

    private static FileTarget CreateFullJsonLogTarget(LoggerTargetSettings targetSettings)
    {
        return new FileTarget("logJsonFile")
        {
            FileName = BuildLogFilePath(targetSettings.FilePath),
            Layout = new JsonLayout
            {
                Attributes =
                {
                    new JsonAttribute("datetime", "${longdate}"),
                    new JsonAttribute("level", "${level:uppercase=true}"),
                    new JsonAttribute("logger", targetSettings.ShortLoggerNames
                        ? "${logger:shortname=true}"
                        : "${logger}"),
                    new JsonAttribute("message", "${message}"),
                    new JsonAttribute("exception", "${exception:format=ToString}"),
                }
            }
        };
    }

    private static ColoredConsoleTarget CreateConsoleLogTarget(LoggerTargetSettings settings)
    {
        return new ColoredConsoleTarget("logConsole")
        {
            Layout = FormatLayout(LoggerLayouts.ConsoleLayout, settings),
            UseDefaultRowHighlightingRules = true,
            RowHighlightingRules =
            {
                ConsoleRowHighlightingRule("level == LogLevel.Trace", ConsoleOutputColor.DarkGray),
                ConsoleRowHighlightingRule("level == LogLevel.Debug", ConsoleOutputColor.Gray),
                ConsoleRowHighlightingRule("level == LogLevel.Info", ConsoleOutputColor.White),
                ConsoleRowHighlightingRule("level == LogLevel.Warn", ConsoleOutputColor.White),
                ConsoleRowHighlightingRule("level == LogLevel.Error", ConsoleOutputColor.Red),
                ConsoleRowHighlightingRule("level == LogLevel.Fatal", ConsoleOutputColor.DarkRed)
            },
            WordHighlightingRules =
            {
                ConsoleWordHighlightingRule("TRAC", ConsoleOutputColor.DarkGray),
                ConsoleWordHighlightingRule("DEBU", ConsoleOutputColor.Gray),
                ConsoleWordHighlightingRule("INFO", ConsoleOutputColor.Green),
                ConsoleWordHighlightingRule("WARN", ConsoleOutputColor.Yellow),
                ConsoleWordHighlightingRule("ERRO", ConsoleOutputColor.Black, ConsoleOutputColor.Red),
                ConsoleWordHighlightingRule("FATA", ConsoleOutputColor.White, ConsoleOutputColor.Red),
                ConsoleWordsSetHighlightingRule(new[] { "true", "false", "yes", "no" }, ConsoleOutputColor.Blue),
                ConsoleWordsSetHighlightingRule(new[] { "null", "none", "not" }, ConsoleOutputColor.DarkMagenta),
                ConsoleWordsSetHighlightingRule(new[] { " GET ", " POST ", " PUT ", " PATCH ", " DELETE " }, ConsoleOutputColor.Red),
                ConsoleWordHighlightingRegexRule(DateTimeRegExp, ConsoleOutputColor.Gray)
            }
        };
    }


    private static Layout FormatLayout(string layout, LoggerTargetSettings loggerTargetSettings)
    {
        return loggerTargetSettings.ShortLoggerNames
            ? layout.Replace("${logger}", "${logger:shortname=true:padding=20}")
            : layout.Replace("${logger}", "${logger:padding=30}");
    }

    private static ConsoleRowHighlightingRule ConsoleRowHighlightingRule(
        string condition,
        ConsoleOutputColor foregroundColor, ConsoleOutputColor backgroundColor = ConsoleOutputColor.NoChange)
    {
        return new ConsoleRowHighlightingRule(ConditionParser.ParseExpression(condition), foregroundColor, backgroundColor);
    }

    private static ConsoleWordHighlightingRule ConsoleWordHighlightingRule(
        string word,
        ConsoleOutputColor foregroundColor, ConsoleOutputColor backgroundColor = ConsoleOutputColor.NoChange)
    {
        return new ConsoleWordHighlightingRule(word, foregroundColor, backgroundColor);
    }

    private static ConsoleWordHighlightingRule ConsoleWordsSetHighlightingRule(
        string[] words,
        ConsoleOutputColor foregroundColor, ConsoleOutputColor backgroundColor = ConsoleOutputColor.NoChange)
    {
        string wordsPattern = string.Join('|', words);
        return ConsoleWordHighlightingRegexRule($@"(?:^|\W)({wordsPattern})(?:$|\W)", foregroundColor, backgroundColor);
    }

    private static ConsoleWordHighlightingRule ConsoleWordHighlightingRegexRule(
        string regex,
        ConsoleOutputColor foregroundColor, ConsoleOutputColor backgroundColor = ConsoleOutputColor.NoChange)
    {
        return new ConsoleWordHighlightingRule()
        {
            Regex = regex,
            ForegroundColor = foregroundColor,
            BackgroundColor = backgroundColor
        };
    }
}