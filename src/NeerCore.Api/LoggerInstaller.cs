using System.Reflection;
using Microsoft.Extensions.Configuration;
using NeerCore.DependencyInjection;
using NeerCore.Extensions;
using NeerCore.Globals;
using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Layouts;
using NLog.Targets;
using NLog.Web;

namespace NeerCore.Api;

public static class LoggerInstaller
{
	private const string DefaultLoggerName = "Program";

	public static string BaseLogsPath { get; set; } = "${basedir}/logs/";


	public static Layout CsvLayout { get; set; } = new CsvLayout
	{
		Delimiter = CsvColumnDelimiterMode.Space,
		WithHeader = false,
		Quoting = CsvQuotingMode.Nothing,
		Columns =
		{
			new("Date&Time", "${longdate}"),
			new("Level", "${level:uppercase=true:padding=5} —"),
			new("Logger", "[${logger:shortname=true}]"),
			new("Message", "${message}"),
			new("Exception", "${exception:format=ToString}"),
		}
	};

	public static Layout FileLayout { get; set; } = "[${longdate}]  ${level:uppercase=true:padding=5} — ${logger}\n${message} ${exception:format=ToString}";
	public static Layout ConsoleLayout { get; set; } = "[${time}] ${level:uppercase=true:padding=5} — ${message} ${exception:format=ToString}";

	public static ILogger InitDefault(string? rootLoggerName = null)
	{
		var configuration = new LoggingConfiguration();

		var logConsole = new ColoredConsoleTarget("logConsole")
		{
			Layout = ConsoleLayout,
			UseDefaultRowHighlightingRules = true,
			RowHighlightingRules =
			{
				ConsoleRowHighlightingRule("level == LogLevel.Trace", ConsoleOutputColor.DarkGray),
				ConsoleRowHighlightingRule("level == LogLevel.Debug", ConsoleOutputColor.DarkGray),
				ConsoleRowHighlightingRule("level == LogLevel.Info", ConsoleOutputColor.Gray),
				ConsoleRowHighlightingRule("level == LogLevel.Warn", ConsoleOutputColor.Gray),
				ConsoleRowHighlightingRule("level == LogLevel.Error", ConsoleOutputColor.Red),
				ConsoleRowHighlightingRule("level == LogLevel.Fatal", ConsoleOutputColor.Red)
			},
			WordHighlightingRules =
			{
				ConsoleWordHighlightingRule("TRACE", ConsoleOutputColor.DarkGray),
				ConsoleWordHighlightingRule("DEBUG", ConsoleOutputColor.Gray),
				ConsoleWordHighlightingRule("INFO", ConsoleOutputColor.Green),
				ConsoleWordHighlightingRule("WARN", ConsoleOutputColor.Yellow),
				ConsoleWordHighlightingRule("ERROR", ConsoleOutputColor.Black, ConsoleOutputColor.Red),
				ConsoleWordHighlightingRule("FATL", ConsoleOutputColor.White, ConsoleOutputColor.Red),
				ConsoleWordsSetHighlightingRule(new[] { "true", "false", "yes", "no" }, ConsoleOutputColor.Blue),
				ConsoleWordsSetHighlightingRule(new[] { "null", "none" }, ConsoleOutputColor.DarkMagenta)
			}
		};
		var logFile = new FileTarget("logFile")
		{
			FileName = BaseLogsPath + "${shortdate}.log",
			Layout = FileLayout
		};
		var logErrorsFile = new FileTarget("logErrorsFile")
		{
			FileName = BaseLogsPath + "${shortdate}-errors.log",
			Layout = FileLayout
		};

		configuration.AddTarget(logConsole);
		configuration.AddTarget(logFile);
		configuration.AddTarget(logErrorsFile);

		string applicationAssembly = GlobalConfiguration.ApplicationNamespace;

		foreach (var target in new Target[] { logConsole, logFile })
		{
			configuration.LoggingRules.Add(new LoggingRule("Microsoft.*", LogLevel.Info, target));
			configuration.LoggingRules.Add(new LoggingRule(applicationAssembly + ".*", LogLevel.Trace, target));
			configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Warn, target));
		}

		configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Warn, logErrorsFile));

		LogManager.Configuration = configuration;

		ConfigurationItemFactory.Default.RegisterItemsFromAssembly(Assembly.Load("NLog.Extensions.Logging"));
		ConfigurationItemFactory.Default.RegisterItemsFromAssembly(Assembly.Load("NLog.Web.AspNetCore"));

		rootLoggerName ??= StackTraceUtility.GetCallerAssembly()!.GetBaseNamespace() + "." + DefaultLoggerName;
		return NLogBuilder.ConfigureNLog(LogManager.Configuration).GetLogger(rootLoggerName);
	}

	public static ILogger InitFromFile(string loggerConfigName = "NLog.json", string rootSectionName = "NLog", string rootLoggerName = DefaultLoggerName)
	{
		var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile(loggerConfigName, optional: true, reloadOnChange: true).Build();

		LogManager.Configuration = new NLogLoggingConfiguration(config.GetRequiredSection(rootSectionName));
		return NLogBuilder.ConfigureNLog(LogManager.Configuration).GetLogger(rootLoggerName);
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