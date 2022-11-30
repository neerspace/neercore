using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.Data.EntityFramework.Extensions;
using NeerCore.Typeids.Data.EntityFramework.Extensions;

namespace EFTest.SqlServer;

public sealed class TestDbContext : DbContext, IDatabase
{
    public TestDbContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyTypedIdsFromAssembly();
        modelBuilder.ApplyAllDataSeeders();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connection = "Server=localhost,1433;Database=NeerCore_EFTest;User=sa;Password=MyPassword1234";
        optionsBuilder.UseSqlServer(connection);
        optionsBuilder.EnableDetailedErrors();
    }
}