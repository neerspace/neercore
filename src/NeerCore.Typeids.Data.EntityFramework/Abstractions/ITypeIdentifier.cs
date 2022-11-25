namespace NeerCore.Typeids.Data.EntityFramework.Abstractions;

public interface ITypeIdentifier<out TValue>
{
    TValue Value { get; }
}