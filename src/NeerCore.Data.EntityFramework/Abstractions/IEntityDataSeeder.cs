using NeerCore.Data.Abstractions;

namespace NeerCore.Data.EntityFramework.Abstractions;

/// <summary>
///   Provides clean API to auto seed your data in DB for single entity.
/// </summary>
/// <typeparam name="TEntity">Entity type to seed.</typeparam>
public interface IEntityDataSeeder<out TEntity>
    where TEntity : class, IEntity
{
    IEnumerable<TEntity> Data { get; }
}