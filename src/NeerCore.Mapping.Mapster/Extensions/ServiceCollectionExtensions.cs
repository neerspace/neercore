using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.Mapping.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>Register mappings from <see cref="TRegister"/>.</summary>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors</param>
    /// <typeparam name="TRegister">Mappings register type.</typeparam>
    public static void RegisterMapper<TRegister>(this IServiceCollection services)
            where TRegister : IRegister
    {
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.RegisterMapper<TRegister>();
    }

    /// <summary>Register mappings from <see cref="TRegister"/>.</summary>
    /// <param name="serviceProvider">Service provider for services presented in <see cref="TRegister"/> DI constructor.</param>
    /// <typeparam name="TRegister">Mappings register type.</typeparam>
    public static void RegisterMapper<TRegister>(this IServiceProvider serviceProvider)
            where TRegister : IRegister
    {
        var paramTypes = typeof(TRegister).GetConstructors()[0].GetParameters().Select(p => p.ParameterType);
        var constructorParams = paramTypes.Select(serviceProvider.GetService).ToArray();

        var register = (TRegister) Activator.CreateInstance(typeof(TRegister), constructorParams)!;
        register.Register(TypeAdapterConfig.GlobalSettings);
    }

    /// <summary>Registers all implementations of <see cref="IRegister"/> interface mappings.</summary>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors</param>
    public static void RegisterMappersFromCurrentAssembly(this IServiceCollection services)
    {
        services.RegisterMappersFromAssembly(Assembly.GetCallingAssembly());
    }

    /// <summary>Registers all implementations of <see cref="IRegister"/> interface mappings.</summary>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors</param>
    /// <param name="assembly">Assembly with <see cref="IRegister"/> implementations to register.</param>
    public static void RegisterMappersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(typeAdapterConfig);
        services.AddScoped<IMapper, Mapper>();
    }
}