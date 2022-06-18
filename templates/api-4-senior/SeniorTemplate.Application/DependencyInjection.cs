using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Application.Extensions;
using NeerCore.Mapping;

namespace SeniorTemplate.Application;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatorApplication("SeniorTemplate.Application");
		services.BindConfigurationOptions(configuration);
		services.RegisterMappings();
	}


	private static void BindConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
	{
		// services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));
	}

	private static void RegisterMappings(this IServiceCollection services)
	{
		services.RegisterMapper<MapperRegister>();
	}
}