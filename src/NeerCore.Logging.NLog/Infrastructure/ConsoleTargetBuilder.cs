using NLog.Conditions;
using NLog.Config;
using NLog.Targets;

namespace NeerCore.Logging.Infrastructure;

public class ConsoleTargetBuilder : TargetBuilderBase
{
    private const string DateTimeRegExp = @"\[(2[0-3]|[01]?[0-9]):([0-5]?[0-9]):([0-5]?[0-9])\.\d\d\d\d\]";

    protected string ConsoleLayout { get; set; } =
        "[${time}] ${logger} — |${level:uppercase=true:truncate=4}| — ${message} ${exception:format=ToString}";

    public override bool Enabled => Settings.Targets.Console.Enabled;


    public override Target Build()
    {
        var targetSettings = Settings.Targets.Console;

        return new ColoredConsoleTarget("logConsole")
        {
            Layout = FormatLayout(ConsoleLayout, targetSettings),
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

    public override void Configure(LoggingConfiguration configuration, Target target)
    {
        configuration.AddTarget(target.Name, target);
        ApplyLogLevelsFromSettings(configuration, target);
    }


    private static ConsoleRowHighlightingRule ConsoleRowHighlightingRule(
        string condition, ConsoleOutputColor foregroundColor, ConsoleOutputColor backgroundColor = ConsoleOutputColor.NoChange)
    {
        return new ConsoleRowHighlightingRule(ConditionParser.ParseExpression(condition), foregroundColor, backgroundColor);
    }

    private static ConsoleWordHighlightingRule ConsoleWordHighlightingRule(
        string word, ConsoleOutputColor foregroundColor, ConsoleOutputColor backgroundColor = ConsoleOutputColor.NoChange)
    {
        return new ConsoleWordHighlightingRule(word, foregroundColor, backgroundColor);
    }

    private static ConsoleWordHighlightingRule ConsoleWordsSetHighlightingRule(
        string[] words, ConsoleOutputColor foregroundColor, ConsoleOutputColor backgroundColor = ConsoleOutputColor.NoChange)
    {
        string wordsPattern = string.Join('|', words);
        return ConsoleWordHighlightingRegexRule($@"(?:^|\W)({wordsPattern})(?:$|\W)", foregroundColor, backgroundColor);
    }

    private static ConsoleWordHighlightingRule ConsoleWordHighlightingRegexRule(
        string regex, ConsoleOutputColor foregroundColor, ConsoleOutputColor backgroundColor = ConsoleOutputColor.NoChange)
    {
        return new ConsoleWordHighlightingRule()
        {
            Regex = regex,
            ForegroundColor = foregroundColor,
            BackgroundColor = backgroundColor
        };
    }
}