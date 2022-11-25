using NeerCore.Typeids.Data.EntityFramework.Abstractions;

namespace NeerCore.Typeids.Data.EntityFramework;

public interface ITypeidsProcessor
{
    string SerializeString<TIdentifier, TValue>(TIdentifier identifier)
        where TIdentifier : ITypeIdentifier<TValue> where TValue : new();

    string SerializeString(object? identifier);

    TIdentifier DeserializeIdentifier<TIdentifier, TValue>(string? stringValue)
        where TIdentifier : ITypeIdentifier<TValue> where TValue : new();

    object? DeserializeIdentifier(string? stringValue, Type targetIdentifier);
}