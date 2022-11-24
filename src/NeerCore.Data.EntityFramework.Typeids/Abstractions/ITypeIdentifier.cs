namespace NeerCore.Data.EntityFramework.Typeids.Abstractions;

public interface ITypeIdentifier<out TValue>
{
    TValue Value { get; }
}