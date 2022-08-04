namespace NeerCore.Data.Abstractions;

/// <summary>
///   Defines an entity with the required <see cref="DateTime">Nullable&lt;DateTime&gt;</see> when entity was
///   last <see cref="IUpdatableEntity.Updated"/> and <see cref="DateTime"/> when it was <see cref="ICreatableEntity.Created"/>.
/// </summary>
public interface IDateableEntity : ICreatableEntity, IUpdatableEntity { }

/// <summary>
///   Defines an entity with the required primary key <see cref="IEntity{TKey}.Id"/> property,
///   <see cref="DateTime">Nullable&lt;DateTime&gt;</see> when entity was last
///   <see cref="IUpdatableEntity.Updated"/> and <see cref="DateTime"/> when it was <see cref="ICreatableEntity.Created"/>.
/// </summary>
/// <typeparam name="TKey"><see cref="IEntity{TKey}.Id"/> type.</typeparam>
public interface IDateableEntity<out TKey> : IDateableEntity, ICreatableEntity<TKey>, IUpdatableEntity<TKey> { }