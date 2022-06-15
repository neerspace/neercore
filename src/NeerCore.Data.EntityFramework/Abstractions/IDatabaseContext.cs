using Microsoft.EntityFrameworkCore;

namespace NeerCore.Data.EntityFramework.Abstractions;

public interface IDatabaseContext
{
	DbSet<TEntity> Set<TEntity>() where TEntity : class;
	Task<int> SaveChangesAsync(CancellationToken cancel = default);
}