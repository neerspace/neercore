using System.Reflection;
using Microsoft.Extensions.Configuration;
using NeerCore.DependencyInjection;
using NeerCore.Extensions;
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
		Delimiter = CsvColumnDelimiterMode.Tab,
		WithHeader = false,
		Quoting = CsvQuotingMode.Nothing,
		Columns =
		{
			new("Date&Time", "${longdate}"),
			new("Level", "${level:uppercase=true:truncate=4}"),
			new("Logger", "[${logger}]"),
			new("Message", "${message}"),
			new("Exception", "${exception:format=ToString}"),
		}
	};

	public static Layout FileLayout { get; set; } = "${longdate} |${level:uppercase=true:truncate=4}| — ${logger:padding=25}[${threadid}]\n${message} ${exception:format=ToString}";
	public static Layout ConsoleLayout { get; set; } = "[${time}] ${logger:shortname=true:padding=25} — |${level:uppercase=true:truncate=4}| — ${message} ${exception:format=ToString}";

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
				ConsoleWordHighlightingRule("TRAC", ConsoleOutputColor.DarkGray),
				ConsoleWordHighlightingRule("DEBU", ConsoleOutputColor.Gray),
				ConsoleWordHighlightingRule("INFO", ConsoleOutputColor.Green),
				ConsoleWordHighlightingRule("WARN", ConsoleOutputColor.Yellow),
				ConsoleWordHighlightingRule("ERRO", ConsoleOutputColor.Black, ConsoleOutputColor.Red),
				ConsoleWordHighlightingRule("FATA", ConsoleOutputColor.White, ConsoleOutputColor.Red),
				ConsoleWordsSetHighlightingRule(new[] { "true", "false", "yes", "no" }, ConsoleOutputColor.Blue),
				ConsoleWordsSetHighlightingRule(new[] { "null", "none", "not" }, ConsoleOutputColor.DarkMagenta),
				ConsoleWordHighlightingRegexRule(@"\[(2[0-3]|[01]?[0-9]):([0-5]?[0-9]):([0-5]?[0-9])\.\d\d\d\d\]", ConsoleOutputColor.DarkGray)
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

		string applicationAssembly = GlobalConfig.ApplicationNamespace;

		foreach (var target in new Target[] { logConsole, logFile })
		{
			configuration.AddRule(LogLevel.Warn, LogLevel.Fatal, target, "Microsoft.EntityFrameworkCore.*", final: true);
			configuration.AddRule(LogLevel.Warn, LogLevel.Fatal, target, "Microsoft.AspNetCore.Hosting.*", final: true);
			configuration.AddRule(LogLevel.Info, LogLevel.Fatal, target, "Microsoft.Hosting.Lifetime.*", final: true);
			configuration.AddRule(LogLevel.Info, LogLevel.Fatal, target, "Microsoft.*", final: true);
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