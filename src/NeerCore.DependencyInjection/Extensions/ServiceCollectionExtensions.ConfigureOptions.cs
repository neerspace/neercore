using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace NeerCore.DependencyInjection.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static void ConfigureAllOptions(this IServiceCollection services)
    {
        services.ConfigureAllOptionsFromAssembly(Assembly.GetCallingAssembly());
    }

    public static void ConfigureAllOptionsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var optionsConfiguratorType in AssemblyProvider.GetImplementationsFromAssembly(typeof(IConfigureOptions<>), assembly))
        {
            services.ConfigureOptions(optionsConfiguratorType);
        }
    }
}