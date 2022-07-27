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
    [Obsolete("Use RegisterMappers overload instead of this.")]
    public static void RegisterMappersFromCurrentAssembly(this IServiceCollection services)
    {
        services.RegisterMappersFromAssembly(Assembly.GetCallingAssembly());
    }

    public static void RegisterAllMappers(this IServiceCollection services)
    {
        services.RegisterMappersFromCurrentAssembly();
    }

    /// <summary>
    ///   Registers all implementations of <see cref="IRegister"/> interface mappings.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors</param>
    /// <param name="assembly">Assembly with <see cref="IRegister"/> implementations to register.</param>
    public static void RegisterMappersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var serviceProvider = services.BuildServiceProvider();
        var registers = AssemblyProvider.GetImplementationsOf<IRegister>();
        foreach (Type mapperRegisterType in registers)
            serviceProvider.RegisterMapper(mapperRegisterType);
        services.AddScoped<IMapper, Mapper>();
    }

    /// <summary>
    ///   Register mappings from <see cref="TRegister"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
    /// <typeparam name="TRegister">Mappings register type.</typeparam>
    public static void RegisterMapper<TRegister>(this IServiceCollection services)
        where TRegister : IRegister
    {
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.RegisterMapper<TRegister>();
        services.AddScoped<IMapper, Mapper>();
    }

    /// <summary>
    ///   Register mappings from provided class type.
    /// </summary>
    /// <param name="mapperRegisterType">Mappings register class.</param>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
    public static void RegisterMapper(this IServiceCollection services, Type mapperRegisterType)
    {
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.RegisterMapper(mapperRegisterType);
        services.AddScoped<IMapper, Mapper>();
    }

    /// <summary>
    ///   Register mappings from <see cref="TRegister"/>.
    /// </summary>
    /// <param name="serviceProvider">Service provider for services presented in <see cref="TRegister"/> DI constructor.</param>
    /// <typeparam name="TRegister">Mappings register type.</typeparam>
    public static void RegisterMapper<TRegister>(this IServiceProvider serviceProvider)
        where TRegister : IRegister
    {
        serviceProvider.RegisterMapper(typeof(TRegister));
    }

    /// <summary>
    ///   Register mappings from provided class type.
    /// </summary>
    /// <param name="mapperRegisterType">Mappings register class.</param>
    /// <param name="serviceProvider">Service provider for services presented in <see cref="mapperRegisterType"/> DI constructor.</param>
    public static void RegisterMapper(this IServiceProvider serviceProvider, Type mapperRegisterType)
    {
        var paramTypes = mapperRegisterType.GetConstructors()[0].GetParameters().Select(p => p.ParameterType);
        var constructorParams = paramTypes.Select(serviceProvider.GetService).ToArray();

        var register = (IRegister)Activator.CreateInstance(mapperRegisterType, constructorParams)!;
        register.Register(TypeAdapterConfig.GlobalSettings);
    }
}