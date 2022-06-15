using Microsoft.Extensions.Configuration;
using NLog;
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
			UseDefaultRowHighlightingRules = true
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

		configuration.AddRule(LogLevel.Trace, LogLevel.Fatal, logConsole);
		configuration.AddRule(LogLevel.Trace, LogLevel.Fatal, logFile);
		configuration.AddRule(LogLevel.Warn, LogLevel.Fatal, logErrorsFile);

		LogManager.Configuration = configuration;
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
}