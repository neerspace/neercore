namespace NeerCore.Data.Abstractions;

/// <summary>
///   Defines an entity with the required <see cref="DateTime"/> when it was <see cref="Created"/>.
/// </summary>
public interface ICreatableEntity : IEntity
{
    /// <summary>
    ///   Gets the <see cref="DateTime"/> when entity was created.
    /// </summary>
    DateTime Created { get; }
}

/// <summary>
///   Defines an entity with the required <see cref="DateTimeOffset"/> when it was <see cref="Created"/>.
/// </summary>
public interface ICreatableOffsetEntity : IEntity
{
    /// <summary>
    ///   Gets the <see cref="DateTimeOffset"/> when entity was created.
    /// </summary>
    DateTimeOffset Created { get; }
}

/// <summary>
///   Defines an entity with the required primary key <see cref="IEntity{TKey}.Id"/> property and
///   <see cref="DateTime"/> when it was <see cref="ICreatableEntity.Created"/>.
/// </summary>
/// <typeparam name="TKey"><see cref="IEntity{TKey}.Id"/> type.</typeparam>
public interface ICreatableEntity<out TKey> : ICreatableEntity, IEntity<TKey> { }

/// <summary>
///   Defines an entity with the required primary key <see cref="IEntity{TKey}.Id"/> property and
///   <see cref="DateTimeOffset"/> when it was <see cref="ICreatableOffsetEntity.Created"/>.
/// </summary>
/// <typeparam name="TKey"><see cref="IEntity{TKey}.Id"/> type.</typeparam>
public interface ICreatableOffsetEntity<out TKey> : ICreatableOffsetEntity, IEntity<TKey> { }