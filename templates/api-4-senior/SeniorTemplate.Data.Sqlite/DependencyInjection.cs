using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Data.EntityFramework.Abstractions;
using SeniorTemplate.Data.Context;
using SeniorTemplate.Data.Entities;

namespace SeniorTemplate.Data;

public static class DependencyInjection
{
	public static void AddSqlServerDatabase(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext();
		services.AddIdentityServices(configuration);
	}

	// =======================================================

	private static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddIdentity<AppUser, AppRole>(configuration.GetRequiredSection("Identity").Bind)
				.AddEntityFrameworkStores<SqliteDbContext>()
				.AddTokenProvider<EmailTokenProvider<AppUser>>("Default");

		services.Configure<PasswordHasherOptions>(option => option.IterationCount = 10000);
	}

	private static void AddDbContext(this IServiceCollection services)
	{
		var contextFactory = new SqliteDbContextFactory();
		services.AddDbContext<SqliteDbContext>(cob => contextFactory.ConfigureContextOptions(cob));

		services.AddScoped<IDatabaseContext, SqliteDbContext>();
	}
}