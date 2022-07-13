using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.DependencyInjection.Extensions;

public static partial class ServiceCollectionExtensions
{
	private static void AddServicesOld(this IServiceCollection services, Type implType, InjectAttribute attr)
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
			default:
				throw new ArgumentOutOfRangeException(nameof(attr.InjectionType), "Invalid injection type.");
		}
	}

	private static void AutoInject(this IServiceCollection services, Type implType, InjectAttribute attr)
	{
		if (implType.GetInterfaces().Length > 0)
			InjectAsInterface(services, implType, attr);
		else if (implType.BaseType is not { })
			InjectAsParentClass(services, implType, attr);
		else
			InjectAsCurrentClass(services, implType, attr);
	}

	private static void InjectAsInterface(this IServiceCollection services, Type implType, InjectAttribute attr)
	{
		attr.ServiceType ??= implType.GetInterfaces().First();
		services.Add(new ServiceDescriptor(attr.ServiceType, implType, attr.Lifetime));
	}

	private static void InjectAsCurrentClass(this IServiceCollection services, Type implType, InjectAttribute attr)
	{
		services.Add(new ServiceDescriptor(implType, implType, attr.Lifetime));
	}

	private static void InjectAsParentClass(this IServiceCollection services, Type implType, InjectAttribute attr)
	{
		services.Add(new ServiceDescriptor(implType.BaseType!, implType, attr.Lifetime));
	}
}