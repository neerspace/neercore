using HashidsNet;
using NeerCore.Typeids.Abstractions;
using NeerCore.Typeids.Internal;

namespace NeerCore.Typeids.Api.Hashids;

public class TypeidsHashidsProcessor : TypeidsProcessorBase
{
    private readonly IHashids _hashids;

    public TypeidsHashidsProcessor(IHashids hashids)
    {
        _hashids = hashids;
    }

    protected override string? Serialize<T>(T? identifier)
        where T : default
    {
        return SerializeString(identifier);
    }

    protected override object? Deserialize(string? stringValue, Type targetIdentifierType)
    {
        stringValue = stringValue?.ToUpper();
        object? value = ParseString(stringValue, targetIdentifierType.GetIdentifierValueType());
        return value is null
            ? default
            : IdentifierFactory.Create(value, targetIdentifierType);
    }

    private object? ParseString(string? stringValue, Type type)
    {
        var valueType = type.GetIdentifierValueType();

        if (string.IsNullOrEmpty(stringValue))
            return null;
        if (valueType == Int16Type)
            return (short)_hashids.DecodeSingle(stringValue);
        if (valueType == Int32Type)
            return _hashids.DecodeSingle(stringValue);
        if (valueType == Int64Type)
            return _hashids.DecodeSingleLong(stringValue);
        if (valueType == StringType)
            return stringValue;
        if (valueType == GuidType)
            return Guid.Parse(stringValue);
        return null;
    }

    private string? SerializeString<T>(T? identifier)
    {
        var valueType = typeof(T).GetIdentifierValueType();

        if (identifier is null)
            return null;
        if (valueType == Int16Type)
            return _hashids.Encode(((ITypeIdentifier<short>)identifier).Value);
        if (valueType == Int32Type)
            return _hashids.Encode(((ITypeIdentifier<int>)identifier).Value);
        if (valueType == Int64Type)
            return _hashids.EncodeLong(((ITypeIdentifier<long>)identifier).Value);
        if (valueType == StringType)
            return ((ITypeIdentifier<string>)identifier).Value;
        if (valueType == GuidType)
            return ((ITypeIdentifier<short>)identifier).Value.ToString();
        return null;
    }
}