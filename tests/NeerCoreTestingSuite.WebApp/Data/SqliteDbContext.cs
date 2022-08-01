using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.Data.EntityFramework.Design;
using NeerCore.Data.EntityFramework.Extensions;

namespace NeerCoreTestingSuite.WebApp.Data;

public sealed class SqliteDbContext : DbContext, IDatabaseContext
{
    public SqliteDbContext(DbContextOptions options) : base(options)
    {
        // Be careful with it!
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ConfigureEntities(options =>
        {
            options.DateTimeKind = DateTimeKind.Utc;
            options.EngineStrategy = DbEngineStrategy.SqlServer;
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.ConfigureDataConversions();
    }
}