using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NeerCore.Api.Extensions;
using NeerCore.Api.Extensions.Swagger;
using NeerCore.DependencyInjection.Extensions;
using NLog;
using NLog.Web;
using Sieve.Services;

namespace NeerCore.Api;

public static class DependencyInjection
{
	public static void AddNeerApi(this WebApplicationBuilder builder, string assemblyName) =>
			builder.AddNeerApi(new[] { assemblyName });

	public static void AddNeerApi(this WebApplicationBuilder builder, IEnumerable<string> assemblyNames)
	{
		builder.Logging.AddNLog();
		builder.Services.AddNeerApiServices(assemblyNames);

		builder.Services.AddControllers(KebabCaseNamingConvention.Use);
	}

	/// <summary>
	/// Changes default logging provider to NLog
	/// </summary>
	public static void AddNLog(this ILoggingBuilder logging)
	{
		logging.ClearProviders();
		logging.AddNLogWeb(LogManager.Configuration);
	}

	public static void AddNeerApiServices(this IServiceCollection services, string assemblyName) =>
			services.AddNeerApiServices(new[] { assemblyName });

	/// <summary>
	/// 
	/// </summary>
	public static void AddNeerApiServices(this IServiceCollection services, IEnumerable<string> assemblyNames)
	{
		services.AddScoped<ISieveProcessor, SieveProcessor>();
		services.AddServicesFromAssemblies(assemblyNames);

		services.AddFactoryMiddlewares();
		services.AddDefaultCorsPolicy();
		services.AddCustomApiVersioning();
		services.ConfigureApiBehaviorOptions();
		services.AddCustomSwagger();
	}
}