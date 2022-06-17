using Microsoft.Extensions.DependencyInjection;
using MiddleTemplate.Data.Context;
using NeerCore.Data.EntityFramework.Abstractions;

namespace MiddleTemplate.Data;

public static class DependencyInjection
{
	public static void AddSqlServerDatabase(this IServiceCollection services)
	{
		services.AddDbContext();
	}

	// =======================================================


	private static void AddDbContext(this IServiceCollection services)
	{
		var contextFactory = new SqliteDbContextFactory();
		services.AddDbContext<SqliteDbContext>(cob => contextFactory.ConfigureContextOptions(cob));

		services.AddScoped<IDatabaseContext, SqliteDbContext>();
	}
}