using Microsoft.Extensions.DependencyInjection;
using NeerCore.DependencyInjection.Extensions;

namespace SeniorTemplate.Infrastructure;

public static class DependencyInjection
{
	public static void AddInfrastructure(this IServiceCollection services)
	{
		services.AddServicesFromCurrentAssembly();
	}
}