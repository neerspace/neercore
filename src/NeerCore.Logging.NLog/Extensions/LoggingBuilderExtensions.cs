using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace NeerCore.Logging.Extensions;

public static class LoggingBuilderExtensions
{
    /// <summary>
    ///   Changes default logging provider to NLog.
    /// </summary>
    public static ILoggingBuilder ConfigureNLogAsDefault(this ILoggingBuilder logging)
    {
        logging.ClearProviders();
        return logging.AddNLogWeb(LogManager.Configuration);
    }
}