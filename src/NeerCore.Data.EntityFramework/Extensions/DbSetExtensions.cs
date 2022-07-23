using Microsoft.EntityFrameworkCore;
using NeerCore.Data.Abstractions;
using NeerCore.Exceptions;

namespace NeerCore.Data.EntityFramework.Extensions;

public static class DbSetExtensions
{
    /// <inheritdoc cref="FindOr404Async{TEntity}(DbSet{TEntity},object?[],CancellationToken)"/>
    public static async Task<TEntity> FindOr404Async<TEntity>(this DbSet<TEntity> dbSet, object? key, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        TEntity? entity = await dbSet.FindAsync(new[] { key }, cancellationToken);
        return entity ?? throw new NotFoundException<TEntity>();
    }

    /// <summary>
    ///   Finds an entity with the given primary key values.
    ///   If an entity with the given primary key values is
    ///   being tracked by the context, then it is returned
    ///   immediately without making a request to the database.
    ///   Otherwise, a query is made to the database for an entity
    ///   with the given primary key values and this entity, if found,
    ///   is attached to the context and returned. If no entity is found,
    ///   then <see cref="NotFoundException{TEntity}"/> is thrown.
    /// </summary>
    /// <param name="dbSet">Entity Framework database set.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete</param>
    /// <typeparam name="TEntity">Entity type that implements <see cref="IEntity"/> interface.</typeparam>
    /// <returns>The entity found, or throw <see cref="NotFoundException{TEntity}"/>.</returns>
    /// <exception cref="NotFoundException{TEntity}">If the entity is not found.</exception>
    public static async Task<TEntity> FindOr404Async<TEntity>(this DbSet<TEntity> dbSet, object?[] keyValues, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        TEntity? entity = await dbSet.FindAsync(keyValues, cancellationToken);
        return entity ?? throw new NotFoundException<TEntity>();
    }
}