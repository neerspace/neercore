using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace NeerCore.Api.Extensions;

public static class LoggingBuilderExtensions
{
    /// <summary>
    ///      Changes default logging provider to NLog.
    /// </summary>
    public static void AddNLog(this ILoggingBuilder logging)
    {
        logging.ClearProviders();
        logging.AddNLogWeb(LogManager.Configuration);
    }
}