using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework.Abstractions;
using TraineeTemplate.Api.Data.Entities;

namespace TraineeTemplate.Api.Data;

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
		builder.Entity<Tea>();

		builder.SeedDefaultData();
	}
}