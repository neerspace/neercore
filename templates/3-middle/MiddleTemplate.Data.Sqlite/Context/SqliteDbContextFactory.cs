using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework;

namespace JuniorTemplate.Data;

public class SqliteDbContextFactory : DbContextFactoryBase<SqliteDbContext>
{
	public override string SelectedConnectionName => "TaclesDev";
	public override string SettingsPath => "../Tacles.Api/appsettings.Secrets.json";


	public override SqlServerDbContext CreateDbContext(string[] args) => new(CreateContextOptions());


	public override void ConfigureContextOptions(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(ConnectionString,
			options => options.MigrationsAssembly(MigrationsAssembly));
	}
}