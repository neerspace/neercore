using System.Reflection;
using Microsoft.Extensions.Configuration;
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


	public static Layout FileLayout { get; set; } = new CsvLayout
	{
		Delimiter = CsvColumnDelimiterMode.Tab,
		WithHeader = false,
		Quoting = CsvQuotingMode.Nothing,
		Columns =
		{
			new("Date&Time", "${time}"),
			new("Level", "|${level:uppercase=true}|"),
			new("Logger", "[${logger:shortname=true}]"),
			new("Message", "${message}"),
			new("Exception", "${exception:format=ToString}"),
		}
	};


	public static ILogger InitDefault(string rootLoggerName = DefaultLoggerName)
	{
		var configuration = new LoggingConfiguration();

		var logConsole = new ColoredConsoleTarget("logConsole")
		{
			Layout = "${time}   |${level:uppercase=true}|   ${message} ${exception:format=ToString}",
			UseDefaultRowHighlightingRules = true,
			RowHighlightingRules =
			{
				ConsoleRowHighlightingRule("level == LogLevel.Trace", ConsoleOutputColor.DarkGray),
				ConsoleRowHighlightingRule("level == LogLevel.Debug", ConsoleOutputColor.DarkGray),
				ConsoleRowHighlightingRule("level == LogLevel.Info", ConsoleOutputColor.Gray),
				ConsoleRowHighlightingRule("level == LogLevel.Warn", ConsoleOutputColor.Yellow),
				ConsoleRowHighlightingRule("level == LogLevel.Error", ConsoleOutputColor.Red),
				ConsoleRowHighlightingRule("level == LogLevel.Fatal", ConsoleOutputColor.DarkRed),
			},
			WordHighlightingRules =
			{
				ConsoleWordHighlightingRule("TRACE", ConsoleOutputColor.Gray),
				ConsoleWordHighlightingRule("DEBUG", ConsoleOutputColor.Gray),
				ConsoleWordHighlightingRule("INFO", ConsoleOutputColor.Green),
				ConsoleWordHighlightingRule("WARN", ConsoleOutputColor.Yellow),
				ConsoleWordHighlightingRule("ERROR", ConsoleOutputColor.Black, ConsoleOutputColor.Red),
				ConsoleWordHighlightingRule("FATAL", ConsoleOutputColor.Black, ConsoleOutputColor.Red),
				ConsoleWordHighlightingRule("(true|false|yes|no)", ConsoleOutputColor.Blue),
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

		string applicationAssembly = Assembly.GetExecutingAssembly().GetName().Name!.Split('.')[0];

		foreach (var loggerTarget in new Target[] { logConsole, logFile })
		{
			configuration.LoggingRules.Add(new LoggingRule("Microsoft.*", LogLevel.Info, loggerTarget));
			configuration.LoggingRules.Add(new LoggingRule(applicationAssembly + ".*", LogLevel.Trace, loggerTarget));
			configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Warn, loggerTarget));
		}

		configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Warn, logErrorsFile));

		LogManager.Configuration = configuration;

		ConfigurationItemFactory.Default.RegisterItemsFromAssembly(Assembly.Load("NLog.Extensions.Logging"));
		ConfigurationItemFactory.Default.RegisterItemsFromAssembly(Assembly.Load("NLog.Web.AspNetCore"));

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

	private static ConsoleWordHighlightingRule ConsoleWordHighlightingRule(string text,
		ConsoleOutputColor foregroundColor, ConsoleOutputColor backgroundColor = ConsoleOutputColor.NoChange)
	{
		return new ConsoleWordHighlightingRule(text, foregroundColor, backgroundColor);
	}
}