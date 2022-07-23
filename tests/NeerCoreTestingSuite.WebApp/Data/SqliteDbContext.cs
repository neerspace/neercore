using Microsoft.EntityFrameworkCore;
using NeerCore.Data;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.Data.EntityFramework.Converters;
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
        // Register all your entities here

        builder.ApplyDataSeeders();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<LocalizedString>().HaveConversion<LocalizedStringConverter>();
    }
}