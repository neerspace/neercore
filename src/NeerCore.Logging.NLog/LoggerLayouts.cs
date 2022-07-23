namespace NeerCore.Logging;

public static class LoggerLayouts
{
    // public static Layout CsvLayout { get; set; } = new CsvLayout
    // {
    //     Delimiter = CsvColumnDelimiterMode.Tab,
    //     WithHeader = false,
    //     Quoting = CsvQuotingMode.Nothing,
    //     Columns =
    //     {
    //         new("Date&Time", "${longdate}"),
    //         new("Level", "${level:uppercase=true:truncate=4}"),
    //         new("Logger", "[${logger:shortname=true}]"),
    //         new("Message", "${message}"),
    //         new("Exception", "${exception:format=ToString}"),
    //     }
    // };

    public static string FileLayout { get; set; } = "${longdate} |${level:uppercase=true:truncate=4}| — ${logger}[${threadid}]\n${message} ${exception:format=ToString}";

    public static string ConsoleLayout { get; set; } = "[${time}] ${logger} — |${level:uppercase=true:truncate=4}| — ${message} ${exception:format=ToString}";
}