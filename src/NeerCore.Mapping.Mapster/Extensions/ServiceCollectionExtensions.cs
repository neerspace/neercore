using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.DependencyInjection;

namespace NeerCore.Mapping.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///   Registers all implementations of <see cref="IRegister"/> interface mappings.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors</param>
    [Obsolete("Use 'RegisterAllMappers' overload instead of this.")]
    public static IServiceCollection RegisterMappersFromCurrentAssembly(this IServiceCollection services)
    {
        return services.RegisterMappersFromAssembly(Assembly.GetCallingAssembly());
    }

    [Obsolete("Use 'RegisterAllMappers' overload instead of this.")]
    public static IServiceCollection RegisterMappers(this IServiceCollection services)
    {
        return services.RegisterMappersFromAssembly(Assembly.GetCallingAssembly());
    }

    public static IServiceCollection RegisterAllMappers(this IServiceCollection services)
    {
        return services.RegisterMappersFromAssembly(Assembly.GetCallingAssembly());
    }

    /// <summary>
    ///   Registers all implementations of <see cref="IRegister"/> interface mappings.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors</param>
    /// <param name="assembly">Assembly with <see cref="IRegister"/> implementations to register.</param>
    public static IServiceCollection RegisterMappersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var serviceProvider = services.BuildServiceProvider();
        var registers = AssemblyProvider.GetImplementationsFromAssembly<IRegister>(assembly);
        foreach (Type mapperRegisterType in registers)
            serviceProvider.RegisterMapper(mapperRegisterType);
        return services.AddScoped<IMapper, Mapper>();
    }

    /// <summary>
    ///   Register mappings from <see cref="TRegister"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
    /// <typeparam name="TRegister">Mappings register type.</typeparam>
    public static IServiceCollection RegisterMapper<TRegister>(this IServiceCollection services)
        where TRegister : IRegister
    {
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.RegisterMapper<TRegister>();
        return services.AddScoped<IMapper, Mapper>();
    }

    /// <summary>
    ///   Register mappings from provided class type.
    /// </summary>
    /// <param name="mapperRegisterType">Mappings register class.</param>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
    public static IServiceCollection RegisterMapper(this IServiceCollection services, Type mapperRegisterType)
    {
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.RegisterMapper(mapperRegisterType);
        return services.AddScoped<IMapper, Mapper>();
    }

    /// <summary>
    ///   Register mappings from <see cref="TRegister"/>.
    /// </summary>
    /// <param name="serviceProvider">Service provider for services presented in <see cref="TRegister"/> DI constructor.</param>
    /// <typeparam name="TRegister">Mappings register type.</typeparam>
    public static IServiceProvider RegisterMapper<TRegister>(this IServiceProvider serviceProvider)
        where TRegister : IRegister
    {
        return serviceProvider.RegisterMapper(typeof(TRegister));
    }

    /// <summary>
    ///   Register mappings from provided class type.
    /// </summary>
    /// <param name="mapperRegisterType">Mappings register class.</param>
    /// <param name="serviceProvider">Service provider for services presented in <see cref="mapperRegisterType"/> DI constructor.</param>
    public static IServiceProvider RegisterMapper(this IServiceProvider serviceProvider, Type mapperRegisterType)
    {
        var paramTypes = mapperRegisterType.GetConstructors()[0].GetParameters().Select(p => p.ParameterType);
        var constructorParams = paramTypes.Select(serviceProvider.GetService).ToArray();

        var register = (IRegister)Activator.CreateInstance(mapperRegisterType, constructorParams)!;
        register.Register(TypeAdapterConfig.GlobalSettings);
        return serviceProvider;
    }
}