using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.Data.EntityFramework.Design;
using NeerCore.Data.EntityFramework.Extensions;
using NeerCore.DependencyInjection;

namespace NeerCoreTestingSuite.WebApp.Data;

public sealed class SqliteDbContext : DbContext, IDatabase
{
    public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options)
    {
        // Disable entity tracking by default
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        // Be careful with it! Remove it for real project
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ConfigureEntities(options =>
        {
            options.DateTimeKind = DateTimeKind.Utc;
            options.EngineStrategy = DbEngineStrategy.SqlServer;
            options.SequentialGuids = true;
            options.DataAssemblies = new[]
            {
                GetType().Assembly,
                AssemblyProvider.TryLoad("NeerCore.Tests")
            };
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.ConfigureDataConversions();
    }
}