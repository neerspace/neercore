using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace NeerCore.Data.EntityFramework.Abstractions;

/// <inheritdoc cref="DbContext"/>
public interface IDatabase
{
    /// <inheritdoc cref="DbContext.Database"/>
    DatabaseFacade Database { get; }


    /// <inheritdoc cref="DbContext.Set{TEntity}()"/>
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    /// <inheritdoc cref="DbContext.Set{TEntity}(string)"/>
    DbSet<TEntity> Set<TEntity>(string name) where TEntity : class;


    /// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)"/>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc cref="DbContext.SaveChangesAsync(bool, CancellationToken)"/>
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);


    /// <inheritdoc cref="DbContext.DisposeAsync()"/>
    ValueTask DisposeAsync();


    /// <inheritdoc cref="DbContext.Entry{TEntity}(TEntity)"/>
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    /// <inheritdoc cref="DbContext.Add{TEntity}(TEntity)"/>
    EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

    /// <inheritdoc cref="DbContext.Attach{TEntity}(TEntity)"/>
    EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;

    /// <inheritdoc cref="DbContext.Update{TEntity}(TEntity)"/>
    EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;

    /// <inheritdoc cref="DbContext.Remove{TEntity}(TEntity)"/>
    EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;


    /// <inheritdoc cref="DbContext.AttachRange(object[])"/>
    void AttachRange(params object[] entities);

    /// <inheritdoc cref="DbContext.AttachRange(IEnumerable{object})"/>
    void AttachRange(IEnumerable<object> entities);

    /// <inheritdoc cref="DbContext.AddRange(object[])"/>
    void AddRange(params object[] entities);

    /// <inheritdoc cref="DbContext.AddRange(IEnumerable{object})"/>
    void AddRange(IEnumerable<object> entities);

    /// <inheritdoc cref="DbContext.UpdateRange(object[])"/>
    void UpdateRange(params object[] entities);

    /// <inheritdoc cref="DbContext.UpdateRange(IEnumerable{object})"/>
    void UpdateRange(IEnumerable<object> entities);

    /// <inheritdoc cref="DbContext.RemoveRange(object[])"/>
    void RemoveRange(params object[] entities);

    /// <inheritdoc cref="DbContext.RemoveRange(IEnumerable{object})"/>
    void RemoveRange(IEnumerable<object> entities);


    /// <inheritdoc cref="DbContext.FindAsync(Type, object[])"/>
    ValueTask<object?> FindAsync(Type entityType, params object?[]? keyValues);

    /// <inheritdoc cref="DbContext.FindAsync{TEntity}(object[])"/>
    ValueTask<TEntity?> FindAsync<TEntity>(object?[]? keyValues) where TEntity : class;
}