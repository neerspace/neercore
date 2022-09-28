using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NeerCore.DependencyInjection.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <inheritdoc cref="AddServicesFromAssembly(IServiceCollection,Assembly,Action{InjectionOptions}?)"/>
    public static IServiceCollection AddServicesFromAssemblies(this IServiceCollection services, IEnumerable<string> assemblyNames, Action<InjectionOptions>? configureOptions = null)
    {
        foreach (string assemblyName in assemblyNames)
            services.AddServicesFromAssembly(assemblyName, configureOptions);
        return services;
    }

    /// <inheritdoc cref="AddServicesFromAssembly(IServiceCollection,Assembly,Action{InjectionOptions}?)"/>
    public static IServiceCollection AddServicesFromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies, Action<InjectionOptions>? configureOptions = null)
    {
        var options = new InjectionOptions();
        configureOptions?.Invoke(options);
        options.ServiceAssemblies = options.ServiceAssemblies is null
            ? assemblies
            : options.ServiceAssemblies.Concat(assemblies);

        return services.AddServices(options);
    }

    /// <inheritdoc cref="AddServicesFromAssembly(IServiceCollection,Assembly,Action{InjectionOptions}?)"/>
    public static IServiceCollection AddAllServices(this IServiceCollection services, Action<InjectionOptions>? configureOptions = null)
    {
        return services.AddServicesFromAssembly(Assembly.GetCallingAssembly(), configureOptions);
    }

    /// <inheritdoc cref="AddServicesFromAssembly(IServiceCollection,Assembly,Action{InjectionOptions}?)"/>
    public static IServiceCollection AddServicesFromAssembly(this IServiceCollection services, string assemblyName, Action<InjectionOptions>? configureOptions = null)
    {
        return services.AddServicesFromAssembly(Assembly.Load(assemblyName), configureOptions);
    }

    /// <summary>Registers all services marked with attribute <see cref="InjectableAttribute"/> to DI container.</summary>
    /// <remarks><b>All services implementations MUST be configured with attribute <see cref="InjectableAttribute"/>.</b></remarks>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configureOptions"></param>
    /// <param name="assembly">Services implementations assembly.</param>
    /// <exception cref="ArgumentOutOfRangeException">If invalid injection type provided.</exception>
    public static IServiceCollection AddServicesFromAssembly(this IServiceCollection services, Assembly assembly, Action<InjectionOptions>? configureOptions = null)
    {
        var options = new InjectionOptions();
        configureOptions?.Invoke(options);
        options.ServiceAssemblies = options.ServiceAssemblies is null
            ? new[] { assembly }
            : options.ServiceAssemblies.Append(assembly);

        return services.AddServices(options);
    }

    private static IServiceCollection AddServices(this IServiceCollection services, InjectionOptions options)
    {
        var serviceProvider = services.BuildServiceProvider();
        var environment = serviceProvider.GetService<IHostEnvironment>();
        string? env = options.Environment ?? environment?.EnvironmentName;

        options.ServiceAssemblies ??= new[] { Assembly.GetEntryAssembly()! };
        IEnumerable<Type> serviceTypes = options.ServiceAssemblies.SelectMany(sa => sa.GetTypes());

        foreach (Type implType in serviceTypes)
        {
            var attributes = implType.GetCustomAttributes<InjectableAttribute>().ToArray();

            if (!attributes.Any()) continue;
            foreach (var attr in attributes)
            {
                if (attr.InjectionType is InjectionType.Default)
                    attr.InjectionType = options.DefaultInjectionType;
                if (attr.Lifetime is InstanceLifetime.Default)
                    attr.Lifetime = options.DefaultLifetime.ToInstanceLifetime();

                // Ignore service if environment is required and current env IS NOT EQUALS service env
                if (IsCurrentEnvironment(attr.Environment, env))
                    services.AddServices(attr, implType);
            }
        }

        return services;
    }

    private static void AddServices(this IServiceCollection services, InjectableAttribute attr, Type implType)
    {
        switch (attr.InjectionType)
        {
            case InjectionType.Auto:
                services.AutoInject(implType, attr);
                break;
            case InjectionType.Interface:
                services.InjectAsInterface(implType, attr);
                break;
            case InjectionType.Self:
                services.InjectAsCurrentClass(implType, attr);
                break;
            case InjectionType.BaseClass:
                services.InjectAsParentClass(implType, attr);
                break;
            case InjectionType.Default:
            default:
                throw new ArgumentOutOfRangeException(nameof(attr.InjectionType), "Invalid injection type.");
        }
    }

    private static bool IsCurrentEnvironment(this string? appEnv, string? attrEnv) =>
        string.IsNullOrEmpty(attrEnv)
        || string.IsNullOrEmpty(appEnv)
        || string.Equals(attrEnv, appEnv, StringComparison.OrdinalIgnoreCase);

    private static void AutoInject(this IServiceCollection services, Type implType, InjectableAttribute attr)
    {
        if (implType.GetInterfaces().Length > 0)
            InjectAsInterface(services, implType, attr);
        else if (implType.BaseType is not { })
            InjectAsParentClass(services, implType, attr);
        else
            InjectAsCurrentClass(services, implType, attr);
    }

    private static void InjectAsInterface(this IServiceCollection services, Type implType, InjectableAttribute attr)
    {
        attr.ServiceType ??= implType.GetInterfaces().First();
        services.Add(new ServiceDescriptor(attr.ServiceType, implType, attr.Lifetime.ToServiceLifetime()));
    }

    private static void InjectAsCurrentClass(this IServiceCollection services, Type implType, InjectableAttribute attr)
    {
        services.Add(new ServiceDescriptor(implType, implType, attr.Lifetime.ToServiceLifetime()));
    }

    private static void InjectAsParentClass(this IServiceCollection services, Type implType, InjectableAttribute attr)
    {
        services.Add(new ServiceDescriptor(implType.BaseType!, implType, attr.Lifetime.ToServiceLifetime()));
    }
}