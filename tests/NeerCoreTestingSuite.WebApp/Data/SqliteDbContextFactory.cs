using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework.Design;

namespace NeerCoreTestingSuite.WebApp.Data;

public class SqliteDbContextFactory : DbContextFactoryBase<SqliteDbContext>
{
    public override string SelectedConnectionName => "Sqlite";


    public override SqliteDbContext CreateDbContext(string[] args) => new(CreateContextOptions());

    public override void ConfigureContextOptions(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(ConnectionString,
            options => options.MigrationsAssembly(MigrationsAssembly));
    }
}