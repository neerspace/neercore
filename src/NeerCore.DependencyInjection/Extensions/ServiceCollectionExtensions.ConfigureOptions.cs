using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace NeerCore.DependencyInjection.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static void ConfigureAllOptions(this IServiceCollection services)
    {
        foreach (var optionsConfiguratorType in AssemblyProvider.GetImplementationsOf(typeof(IConfigureOptions<>)))
        {
            services.ConfigureOptions(optionsConfiguratorType);
        }
    }
}