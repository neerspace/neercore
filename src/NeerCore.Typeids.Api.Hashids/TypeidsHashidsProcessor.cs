using HashidsNet;
using NeerCore.DependencyInjection;
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
        return identifier?.ToString();
    }

    protected override object? Deserialize(string? stringValue, Type targetIdentifierType)
    {
        stringValue = stringValue?.ToUpper();
        object? value = ParseString(stringValue, targetIdentifierType.GetIdentifierValueType());
        return value is null ? default : IdentifierFactory.Create(value, targetIdentifierType);
    }

    private object? ParseString(string? stringValue, Type type)
    {
        var valueType = type.GetIdentifierValueType();

        if (string.IsNullOrEmpty(stringValue)) return null;
        if (valueType == Int16Type) return (short)_hashids.DecodeSingle(stringValue);
        if (valueType == Int32Type) return _hashids.DecodeSingle(stringValue);
        if (valueType == Int64Type) return _hashids.DecodeSingleLong(stringValue);
        if (valueType == StringType) return stringValue;
        if (valueType == GuidType) return Guid.Parse(stringValue);
        return null;
    }
}