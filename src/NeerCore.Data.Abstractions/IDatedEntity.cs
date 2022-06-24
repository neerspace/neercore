namespace NeerCore.Data.Abstractions;

public interface IDatedEntity : IEntity
{
	DateTime? Updated { get; }
	DateTime Created { get; }
}

public interface IDatedEntity<out TKey> : IDatedEntity, IEntity<TKey> { }