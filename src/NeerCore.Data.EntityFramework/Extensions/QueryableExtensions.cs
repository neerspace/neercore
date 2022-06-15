using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NeerCore.Data.Abstractions;
using NeerCore.Exceptions;

namespace NeerCore.Data.EntityFramework.Extensions;

public static class QueryableExtensions
{
	public static async Task<TEntity> FirstOr404Async<TEntity>(this IQueryable<TEntity> queryable, CancellationToken cancel = default)
			where TEntity : class, IEntity
	{
		return await queryable.FirstOrDefaultAsync(cancel)
		       ?? throw new NotFoundException<TEntity>();
	}

	public static async Task<TEntity> FirstOr404Async<TEntity>(this IQueryable<TEntity> queryable, Expression<Func<TEntity, bool>> predicate, CancellationToken cancel = default)
			where TEntity : class, IEntity
	{
		return await queryable.FirstOrDefaultAsync(predicate, cancel)
		       ?? throw new NotFoundException<TEntity>();
	}


	public static IQueryable<TEntity> IncludeMany<TEntity>(this IQueryable<TEntity> queryable, params string[] inclusions)
			where TEntity : class, IEntity
	{
		if (inclusions.Length <= 0)
			return queryable;

		foreach (string inclusion in inclusions)
			queryable = queryable.Include(inclusion);
		return queryable;
	}
}