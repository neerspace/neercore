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
    /// <remarks>
    ///   If you use one of the injection methods for your mapping registers,
    ///   you also could be able to use a DI in your <see cref="IRegister"/> implementations.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
    public static IServiceCollection AddAllMappers(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var registers = AssemblyProvider.GetImplementationsOf<IRegister>();
        foreach (Type mapperRegisterType in registers)
            serviceProvider.AddMapperRegister(mapperRegisterType);
        return services.AddScoped<IMapper, Mapper>();
    }

    /// <inheritdoc cref="AddAllMappers"/>
    /// <param name="assembly">Assembly with <see cref="IRegister"/> implementations to register.</param>
    public static IServiceCollection AddMappersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var serviceProvider = services.BuildServiceProvider();
        var registers = AssemblyProvider.GetImplementationsFromAssembly<IRegister>(assembly);
        foreach (Type mapperRegisterType in registers)
            serviceProvider.AddMapperRegister(mapperRegisterType);
        return services.AddScoped<IMapper, Mapper>();
    }

    /// <summary>
    ///   Register mappings from <see cref="TRegister"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
    /// <typeparam name="TRegister">Mappings register type.</typeparam>
    public static IServiceCollection AddMapperRegister<TRegister>(this IServiceCollection services)
        where TRegister : IRegister
    {
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.AddMapperRegister<TRegister>();
        return services.AddScoped<IMapper, Mapper>();
    }

    /// <summary>
    ///   Register mappings from provided class type.
    /// </summary>
    /// <param name="mapperRegisterType">Mappings register class.</param>
    /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
    public static IServiceCollection AddMapperRegister(this IServiceCollection services, Type mapperRegisterType)
    {
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.AddMapperRegister(mapperRegisterType);
        return services.AddScoped<IMapper, Mapper>();
    }

    /// <summary>
    ///   Register mappings from <see cref="TRegister"/>.
    /// </summary>
    /// <param name="serviceProvider">Service provider for services presented in <see cref="TRegister"/> DI constructor.</param>
    /// <typeparam name="TRegister">Mappings register type.</typeparam>
    public static IServiceProvider AddMapperRegister<TRegister>(this IServiceProvider serviceProvider)
        where TRegister : IRegister
    {
        return serviceProvider.AddMapperRegister(typeof(TRegister));
    }

    /// <summary>
    ///   Register mappings from provided class type.
    /// </summary>
    /// <param name="mapperRegisterType">Mappings register class.</param>
    /// <param name="serviceProvider">Service provider for services presented in <see cref="mapperRegisterType"/> DI constructor.</param>
    public static IServiceProvider AddMapperRegister(this IServiceProvider serviceProvider, Type mapperRegisterType)
    {
        var paramTypes = mapperRegisterType.GetConstructors()[0].GetParameters().Select(p => p.ParameterType);
        var constructorParams = paramTypes.Select(serviceProvider.GetService).ToArray();

        var register = (IRegister)Activator.CreateInstance(mapperRegisterType, constructorParams)!;
        register.Register(TypeAdapterConfig.GlobalSettings);
        return serviceProvider;
    }
}