namespace NeerCore.Data.Abstractions;

/// <summary>
///      Defines entity with the required last    <see cref="Updated"/>
///      and <see cref="Created"/> dates.
/// </summary>
public interface IDatedEntity : IEntity
{
    DateTime? Updated { get; }
    DateTime Created { get; }
}

/// <summary>
///      Defines entity with the required <see cref="IEntity{TKey}.Id"/> property,
///      last    <see cref="IDatedEntity.Updated"/>    and <see cref="IDatedEntity.Created"/> dates.
/// </summary>
/// <typeparam name="TKey"><see cref="IEntity{TKey}.Id"/> type.</typeparam>
public interface IDatedEntity<out TKey> : IDatedEntity, IEntity<TKey> { }