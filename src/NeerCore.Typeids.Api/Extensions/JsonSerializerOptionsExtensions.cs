using System.Text.Json;

namespace NeerCore.Typeids.Api.Extensions;

public static class JsonSerializerOptionsExtensions
{
    public static IServiceProvider GetServiceProvider(this JsonSerializerOptions options)
    {
        return options.Converters.OfType<IServiceProvider>().FirstOrDefault()
               ?? throw new InvalidOperationException("ServiceProvider not found");
    }
}