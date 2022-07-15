namespace NeerCore.Data.Abstractions;

/// <summary>Defines entity.</summary>
public interface IEntity { }

/// <summary>Defines entity with the required <see cref="Id"/> property.</summary>
/// <typeparam name="TKey"><see cref="Id"/> type.</typeparam>
public interface IEntity<out TKey> : IEntity
{
    TKey Id { get; }
}