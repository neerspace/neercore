namespace NeerCore.Data.Abstractions;

public interface IEntity { }

public interface IEntity<out TKey> : IEntity
{
	TKey Id { get; }
}