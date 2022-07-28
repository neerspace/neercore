using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace NeerCore.Logging.Extensions;

public static class LoggingBuilderExtensions
{
    /// <summary>
    ///   Changes default logging provider to NLog.
    /// </summary>
    public static void ConfigureNLogAsDefault(this ILoggingBuilder logging)
    {
        logging.ClearProviders();
        logging.AddNLogWeb(LogManager.Configuration);
    }
}