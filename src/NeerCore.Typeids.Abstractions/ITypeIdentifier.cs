namespace NeerCore.Typeids.Abstractions;

public interface ITypeIdentifier<out TValue>
{
    TValue Value { get; }
}