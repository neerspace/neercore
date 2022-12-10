using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NeerCore.Logging.Extensions;

public static class ServiceCollectionExtensions
{
    public static ILogger GetLoggerOfType(this IServiceProvider provider, Type type) =>
        provider.GetRequiredService<ILoggerFactory>().CreateLogger(type);
}