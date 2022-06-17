using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.Mapping;

public static class DependencyInjection
{
	public static void RegisterMapper<TRegister>(this IServiceCollection services)
			where TRegister : IRegister
	{
		var serviceProvider = services.BuildServiceProvider();
		serviceProvider.RegisterMapper<TRegister>();
	}

	public static void RegisterMapper<TRegister>(this IServiceProvider serviceProvider)
			where TRegister : IRegister
	{
		var paramTypes = typeof(TRegister).GetConstructors()[0].GetParameters().Select(p => p.ParameterType);
		var constructorParams = paramTypes.Select(serviceProvider.GetService).ToArray();

		var register = (TRegister) Activator.CreateInstance(typeof(TRegister), constructorParams)!;
		register.Register(TypeAdapterConfig.GlobalSettings);
	}

	public static void RegisterMappers(this IServiceCollection services)
	{
		var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
		typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
	}
}