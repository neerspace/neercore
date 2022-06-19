using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Api.Extensions.Swagger;
using NeerCore.DependencyInjection.Extensions;
using Sieve.Services;

namespace NeerCore.Api.Extensions;

public static class ServiceCollectionExtensions
{
	public static void AddNeerApiServices(this IServiceCollection services, params string[] assemblyNames) =>
			services.AddNeerApiServices(assemblyNames.Select(Assembly.Load).ToArray());

	public static void AddNeerApiServices(this IServiceCollection services, params Assembly[] assemblies)
	{
		services.AddScoped<ISieveProcessor, SieveProcessor>();
		services.AddServicesFromAssemblies(assemblies);

		services.AddFactoryMiddlewares();
		services.AddDefaultCorsPolicy();
		services.AddCustomApiVersioning();
		services.ConfigureApiBehaviorOptions();
		services.AddCustomSwagger();
	}
}