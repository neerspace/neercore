using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework.Design;

namespace NeerCoreTestingSuite.WebApp.Data;

public class SqliteDbContextFactory : DbContextFactoryBase<SqliteDbContext>
{
    // public override string SelectedConnectionName => "Sqlite";
    public override string SelectedConnectionName => "SqlServer";

    public override string[] SettingsPaths => new[] { "invalid.path.json", "appsettings.Development.json" };


    public override SqliteDbContext CreateDbContext(string[] args) => new(CreateContextOptions());

    public override void ConfigureContextOptions(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString,
            options => options.MigrationsAssembly(MigrationsAssembly));
    }
}