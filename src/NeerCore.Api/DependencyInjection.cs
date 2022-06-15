using System.Reflection;
using Mapster;
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
	public static void AddNeerApi(this WebApplicationBuilder builder, params string[] assemblyNames)
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

	public static void RegisterMapper<TRegister>(this IServiceCollection services)
			where TRegister : IRegister
	{
		var serviceProvider = services.BuildServiceProvider();
		var paramTypes = typeof(TRegister).GetConstructors()[0].GetParameters().Select(p => p.ParameterType);
		var constructorParams = paramTypes.Select(paramType => serviceProvider.GetService(paramType)).ToArray();

		var register = (TRegister) Activator.CreateInstance(typeof(TRegister), constructorParams)!;
		register.Register(TypeAdapterConfig.GlobalSettings);
	}

	public static void RegisterMappers(this IServiceCollection services)
	{
		var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
		typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
	}

	/// <summary>
	/// 
	/// </summary>
	public static void AddNeerApiServices(this IServiceCollection services, params string[] assemblyNames)
	{
		services.AddScoped<ISieveProcessor, SieveProcessor>();
		services.AddServicesFromAssemblies(assemblyNames);

		services.AddFactoryMiddleware();
		services.AddDefaultCorsPolicy();
		services.AddCustomApiVersioning();
		services.ConfigureApiBehaviorOptions();
		services.AddCustomSwagger();
	}
}