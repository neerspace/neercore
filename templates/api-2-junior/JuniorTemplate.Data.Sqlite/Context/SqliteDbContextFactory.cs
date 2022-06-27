using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework;
using NeerCore.Data.EntityFramework.Design;

namespace JuniorTemplate.Data.Context;

public class SqliteDbContextFactory : DbContextFactoryBase<SqliteDbContext>
{
	public override string SelectedConnectionName => "Sqlite";
	public override string SettingsPath => "../SeniorTemplate.Api/appsettings.Secrets.json";

	public override SqliteDbContext CreateDbContext(string[] args) => new(CreateContextOptions());


	public override void ConfigureContextOptions(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite(ConnectionString,
			options => options.MigrationsAssembly(MigrationsAssembly));
	}
}