using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.DependencyInjection.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <inheritdoc cref="AddServicesFromAssembly(IServiceCollection,Assembly,Action{InjectionOptions}?)"/>
    public static void AddServicesFromAssemblies(this IServiceCollection services, IEnumerable<string> assemblyNames, Action<InjectionOptions>? configureOptions = null)
    {
        foreach (string assemblyName in assemblyNames)
            services.AddServicesFromAssembly(assemblyName, configureOptions);
    }

    /// <inheritdoc cref="AddServicesFromAssembly(IServiceCollection,Assembly,Action{InjectionOptions}?)"/>
    public static void AddServicesFromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies, Action<InjectionOptions>? configureOptions = null)
    {
        var options = new InjectionOptions();
        configureOptions?.Invoke(options);
        options.ServiceAssemblies = options.ServiceAssemblies is null
            ? assemblies
            : options.ServiceAssemblies.Concat(assemblies);

        services.AddServices(options);
    }

    /// <inheritdoc cref="AddServicesFromAssembly(IServiceCollection,Assembly,Action{InjectionOptions}?)"/>
    public static void AddServicesFromCurrentAssembly(this IServiceCollection services, Action<InjectionOptions>? configureOptions = null)
    {
        services.AddServicesFromAssembly(Assembly.GetCallingAssembly(), configureOptions);
    }

    /// <inheritdoc cref="AddServicesFromAssembly(IServiceCollection,Assembly,Action{InjectionOptions}?)"/>
    public static void AddServicesFromAssembly(this IServiceCollection services, string assemblyName, Action<InjectionOptions>? configureOptions = null)
    {
        services.AddServicesFromAssembly(Assembly.Load(assemblyName), configureOptions);
    }

    /// <param name="assembly">Services implementations assembly.</param>
    /// <inheritdoc cref="AddServices(IServiceCollection,Action{InjectionOptions}?)"/>
    public static void AddServicesFromAssembly(this IServiceCollection services, Assembly assembly, Action<InjectionOptions>? configureOptions = null)
    {
        var options = new InjectionOptions();
        configureOptions?.Invoke(options);
        options.ServiceAssemblies = options.ServiceAssemblies is null
            ? new[] { assembly }
            : options.ServiceAssemblies.Append(assembly);

        services.AddServices(options);
    }

    /// <summary>Registers all services marked with attribute <see cref="InjectAttribute"/> to DI container.</summary>
    /// <remarks><b>All services implementations MUST be configured with attribute <see cref="InjectAttribute"/>.</b></remarks>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configureOptions"></param>
    /// <exception cref="ArgumentOutOfRangeException">If invalid injection type provided.</exception>
    public static void AddServices(this IServiceCollection services, Action<InjectionOptions>? configureOptions = null)
    {
        var options = new InjectionOptions();
        configureOptions?.Invoke(options);
        services.AddServices(options);
    }


    private static void AddServices(this IServiceCollection services, InjectionOptions options)
    {
        options.ServiceAssemblies ??= new[] { Assembly.GetCallingAssembly() };
        IEnumerable<Type> serviceTypes = options.ServiceAssemblies.SelectMany(sa => sa.GetTypes());

        foreach (Type implType in serviceTypes)
        {
            var attr = implType.GetAttribute<InjectableAttribute>();
            if (attr is null)
            {
                // TODO: Remove in next version
                InjectAttribute attr2 = implType.GetAttribute<InjectAttribute>();
                if (attr is not null) services.AddServicesOld(implType, attr2);
                continue;
            }

            if (attr.InjectionType is InjectionType.Default)
                attr.InjectionType = options.DefaultInjectionType;
            if (attr.Lifetime is InstanceLifetime.Default)
                attr.Lifetime = options.DefaultLifetime.ToInstanceLifetime();

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
                default:
                    throw new ArgumentOutOfRangeException(nameof(attr.InjectionType), "Invalid injection type.");
            }
        }
    }

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