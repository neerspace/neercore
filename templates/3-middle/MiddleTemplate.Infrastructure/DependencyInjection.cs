using Microsoft.Extensions.DependencyInjection;
using NeerCore.DependencyInjection.Extensions;

namespace MiddleTemplate.Infrastructure;

public static class DependencyInjection
{
	public static void AddInfrastructure(this IServiceCollection services)
	{
		services.AddServicesFromAssembly("MiddleTemplate.Infrastructure");
	}
}