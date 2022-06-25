using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
	/// <inheritdoc cref="AddServicesFromAssembly(IServiceCollection,Assembly)"/>
	public static void AddServicesFromAssemblies(this IServiceCollection services, IEnumerable<string> assemblyNames)
	{
		foreach (string assemblyName in assemblyNames)
			services.AddServicesFromAssembly(assemblyName);
	}

	/// <inheritdoc cref="AddServicesFromAssembly(IServiceCollection,Assembly)"/>
	public static void AddServicesFromAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies)
	{
		foreach (Assembly assembly in assemblies)
			services.AddServicesFromAssembly(assembly);
	}

	/// <inheritdoc cref="AddServicesFromAssembly(IServiceCollection,Assembly)"/>
	public static void AddServicesFromCurrentAssembly(this IServiceCollection services)
	{
		services.AddServicesFromAssembly(StackTraceUtility.GetRequiredCallerAssembly());
	}

	/// <inheritdoc cref="AddServicesFromAssembly(IServiceCollection,Assembly)"/>
	public static void AddServicesFromAssembly(this IServiceCollection services, string assemblyName)
	{
		services.AddServicesFromAssembly(Assembly.Load(assemblyName));
	}

	/// <summary>Registers all services marked with attribute <see cref="InjectAttribute"/> to DI container.</summary>
	/// <remarks><b>All services implementations MUST be configured with attribute <see cref="InjectAttribute"/>.</b></remarks>
	/// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
	/// <param name="assembly">Services implementations assembly.</param>
	/// <exception cref="ArgumentOutOfRangeException">If invalid injection type provided.</exception>
	public static void AddServicesFromAssembly(this IServiceCollection services, Assembly assembly)
	{
		IEnumerable<Type> serviceTypes = assembly.GetTypes();

		foreach (Type implType in serviceTypes)
		{
			var attr = implType.GetAttribute<InjectAttribute>();
			if (attr is null)
				continue;

			switch (attr.InjectionType)
			{
				case InjectionType.Auto:
				{
					if (implType.GetInterfaces().Length > 0)
						goto case InjectionType.Interface;
					if (implType.BaseType is not { })
						goto case InjectionType.BaseClass;
					goto case InjectionType.Self;
				}
				case InjectionType.Interface:
				{
					attr.ServiceType ??= implType.GetInterfaces().First();
					services.Add(new ServiceDescriptor(attr.ServiceType, implType, attr.Lifetime));
					break;
				}
				case InjectionType.Self:
				{
					services.Add(new ServiceDescriptor(implType, implType, attr.Lifetime));
					break;
				}
				case InjectionType.BaseClass:
				{
					services.Add(new ServiceDescriptor(implType.BaseType!, implType, attr.Lifetime));
					break;
				}
				default:
					throw new ArgumentOutOfRangeException(nameof(attr.InjectionType), "Invalid injection type.");
			}
		}
	}
}