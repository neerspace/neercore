using NeerCore.Typeids.Abstractions;

namespace NeerCore.Typeids.Api;

public abstract class TypeidsProcessorBase : ITypeidsProcessor
{
    protected static readonly Type Int16Type = typeof(short);
    protected static readonly Type Int32Type = typeof(int);
    protected static readonly Type Int64Type = typeof(long);
    protected static readonly Type StringType = typeof(string);
    protected static readonly Type GuidType = typeof(Guid);

    protected abstract string? Serialize<T>(T? identifier);
    protected abstract object? Deserialize(string? stringValue, Type targetIdentifierType);

    public string? SerializeString<TIdentifier, TValue>(TIdentifier identifier)
        where TIdentifier : ITypeIdentifier<TValue>
    {
        return Serialize(identifier);
    }

    public string? SerializeString(object? identifier)
    {
        return Serialize(identifier);
    }

    public TIdentifier? DeserializeIdentifier<TIdentifier, TValue>(string? stringValue)
        where TIdentifier : ITypeIdentifier<TValue>
    {
        return (TIdentifier?)Deserialize(stringValue, typeof(TIdentifier));
    }

    public object? DeserializeIdentifier(string? stringValue, Type targetIdentifier)
    {
        return Deserialize(stringValue, targetIdentifier);
    }
}