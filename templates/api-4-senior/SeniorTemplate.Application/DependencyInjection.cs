using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NeerCore.Application.Extensions;
using NeerCore.Mapping;
using NeerCore.Mapping.Extensions;
using SeniorTemplate.Application.Options;

namespace SeniorTemplate.Application;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatorApplicationFromCurrentAssembly();
		services.AddHashids(configuration.GetSection("Hashids").Bind);
		services.BindConfigurationOptions(configuration);
		services.RegisterMappings();
	}


	private static void BindConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtOptions>(options =>
		{
			options.Secret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]));
			options.AccessTokenLifetime = TimeSpan.TryParse(configuration["Jwt:AccessTokenLifetime"], out var val) ? val : TimeSpan.FromMinutes(10);
			options.RefreshTokenLifetime = TimeSpan.TryParse(configuration["Jwt:RefreshTokenLifetime"], out val) ? val : TimeSpan.FromDays(30);
		});
	}

	private static void RegisterMappings(this IServiceCollection services)
	{
		services.RegisterMapper<MapperRegister>();
	}
}