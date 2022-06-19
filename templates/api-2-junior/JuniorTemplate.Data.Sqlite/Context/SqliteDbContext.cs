using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework.Abstractions;

namespace JuniorTemplate.Data.Context;

public class SqliteDbContext : DbContext, IDatabaseContext
{
	public SqliteDbContext(DbContextOptions options) : base(options) { }


	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
		builder.SeedDefaultData();
	}
}