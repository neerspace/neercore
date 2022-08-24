using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace NeerCore.DependencyInjection.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureAllOptions(this IServiceCollection services)
    {
        return services.ConfigureAllOptionsFromAssembly(Assembly.GetCallingAssembly());
    }

    public static IServiceCollection ConfigureAllOptionsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var optionsConfiguratorType in AssemblyProvider.GetImplementationsFromAssembly(typeof(IConfigureOptions<>), assembly))
        {
            services.ConfigureOptions(optionsConfiguratorType);
        }

        return services;
    }
}