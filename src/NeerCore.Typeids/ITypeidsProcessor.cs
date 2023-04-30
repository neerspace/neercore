using NeerCore.Typeids.Abstractions;

namespace NeerCore.Typeids;

public interface ITypeidsProcessor
{
    string? SerializeString<TIdentifier, TValue>(TIdentifier identifier)
        where TIdentifier : ITypeIdentifier<TValue>;

    string? SerializeString(object? identifier);

    TIdentifier? DeserializeIdentifier<TIdentifier, TValue>(string? stringValue)
        where TIdentifier : ITypeIdentifier<TValue>;

    object? DeserializeIdentifier(string? stringValue, Type targetIdentifier);
}