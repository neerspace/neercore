using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Application.Extensions;
using NeerCore.Mapping;

namespace MiddleTemplate.Application;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatorApplication("MiddleTemplate.Application");
		services.BindConfigurationOptions(configuration);
		services.RegisterMappings();
	}


	private static void BindConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
	{
		// services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));
	}

	public static void RegisterMappings(this IServiceCollection services)
	{
		services.RegisterMapper<MapperRegister>();
	}
}