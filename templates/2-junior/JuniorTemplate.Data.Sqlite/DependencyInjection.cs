using Microsoft.Extensions.DependencyInjection;
using NeerCore.Data.EntityFramework.Abstractions;

namespace JuniorTemplate.Data;

public static class DependencyInjection
{
	public static void AddSqlServerDatabase(this IServiceCollection services)
	{
		services.AddDbContext();
	}

	// =======================================================


	private static void AddDbContext(this IServiceCollection services)
	{
		var contextFactory = new SqlServerDbContextFactory();
		services.AddDbContext<SqliteDbContext>(cob => contextFactory.ConfigureContextOptions(cob));

		services.AddScoped<IDatabaseContext, SqlServerDbContext>();
	}
}