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
    private static readonly LoggingSettings Settings = new();

    private const string DateTimeRegExp = @"\[(2[0-3]|[01]?[0-9]):([0-5]?[0-9]):([0-5]?[0-9])\.\d\d\d\d\]";

    internal static void Configure(Action<LoggingSettings>? configureOptions = null)
    {
        configureOptions?.Invoke(Settings);
        if (!Settings.LogLevel.ContainsKey("*"))
            Settings.LogLevel.Add("*", "Warning");

        var configuration = new LoggingConfiguration();

        if (Settings.ConsoleLogger is { Enabled: true })
        {
            ColoredConsoleTarget logConsole = CreateConsoleLogTarget();
            configuration.AddTarget(nameof(logConsole), logConsole!);
            ApplyLogLevelsFromSettings(configuration, logConsole!);
        }

        if (Settings.FullFileLogger is { Enabled: true })
        {
            FileTarget logFile = CreateFullFileLogTarget();
            configuration.AddTarget(nameof(logFile), logFile);
            ApplyLogLevelsFromSettings(configuration, logFile);
        }

        if (Settings.ErrorFileLogger is { Enabled: true })
        {
            FileTarget logErrorsFile = CreateErrorFileLogTarget();
            configuration.AddTarget(nameof(logErrorsFile), logErrorsFile);
            configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Warn, logErrorsFile));
        }

        ConfigurationItemFactory.Default.RegisterItemsFromAssembly(Assembly.Load("NLog.Extensions.Logging"));
        ConfigurationItemFactory.Default.RegisterItemsFromAssembly(Assembly.Load("NLog.Web.AspNetCore"));
        LogManager.Configuration = configuration;
    }

    private static void ApplyLogLevelsFromSettings(LoggingConfiguration configuration, Target target)
    {
        // string applicationNamespace = Assembly.GetEntryAssembly()!.GetBaseNamespace();

        foreach (var logLevel in Settings.LogLevel)
        {
            // TODO: Explore more about final rules
            bool isFinal = false; // logLevel.Key != "*" && !logLevel.Key.StartsWith(applicationNamespace);
            configuration.AddRule(ParseLogLevel(logLevel.Value), LogLevel.Fatal, target, logLevel.Key, final: isFinal);
        }
    }

    private static LogLevel ParseLogLevel(string logLevelValue) => logLevelValue switch
    {
        "Trace" => LogLevel.Trace,
        "Debug" => LogLevel.Debug,
        "Information" or "Info" => LogLevel.Info,
        "Warning" or "Warn" => LogLevel.Warn,
        "Error" => LogLevel.Error,
        "Critical" or "Fatal" => LogLevel.Fatal,
        "None" or "Off" => LogLevel.Off,
        _ => throw new InvalidLogLevelException(logLevelValue)
    };

    private static string BuildLogFilePath(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException(nameof(fileName), "File name is not invalid.");

        string path = Settings.Shared.LogsDirectoryPath + fileName;

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

    private static FileTarget CreateErrorFileLogTarget()
    {
        return new FileTarget("logErrorsFile")
        {
            FileName = BuildLogFilePath(Settings.ErrorFileLogger.FilePath),
            Layout = FormatLayout(LoggerLayouts.FileLayout, Settings.ErrorFileLogger)
        };
    }

    private static FileTarget CreateFullFileLogTarget()
    {
        return new FileTarget("logFile")
        {
            FileName = BuildLogFilePath(Settings.FullFileLogger.FilePath),
            Layout = FormatLayout(LoggerLayouts.FileLayout, Settings.FullFileLogger)
        };
    }

    private static ColoredConsoleTarget CreateConsoleLogTarget()
    {
        return new ColoredConsoleTarget("logConsole")
        {
            Layout = FormatLayout(LoggerLayouts.ConsoleLayout, Settings.ConsoleLogger),
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

    private static Layout FormatLayout(string layout, SingleLoggerSettings loggerSettings)
    {
        return loggerSettings.ShortLoggerNames
            ? layout.Replace("${logger}", "${logger:shortname=true:padding=20}")
            : layout.Replace("${logger}", "${logger:padding=30}");
    }

    private static ConsoleRowHighlightingRule ConsoleRowHighlightingRule(string condition,
        ConsoleOutputColor foregroundColor, ConsoleOutputColor backgroundColor = ConsoleOutputColor.NoChange)
    {
        return new ConsoleRowHighlightingRule(ConditionParser.ParseExpression(condition), foregroundColor, backgroundColor);
    }

    private static ConsoleWordHighlightingRule ConsoleWordHighlightingRule(string word,
        ConsoleOutputColor foregroundColor, ConsoleOutputColor backgroundColor = ConsoleOutputColor.NoChange)
    {
        return new ConsoleWordHighlightingRule(word, foregroundColor, backgroundColor);
    }

    private static ConsoleWordHighlightingRule ConsoleWordsSetHighlightingRule(string[] words,
        ConsoleOutputColor foregroundColor, ConsoleOutputColor backgroundColor = ConsoleOutputColor.NoChange)
    {
        string wordsPattern = string.Join('|', words);
        return ConsoleWordHighlightingRegexRule($@"(?:^|\W)({wordsPattern})(?:$|\W)", foregroundColor, backgroundColor);
    }

    private static ConsoleWordHighlightingRule ConsoleWordHighlightingRegexRule(string regex,
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