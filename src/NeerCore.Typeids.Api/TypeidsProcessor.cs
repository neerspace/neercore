using NeerCore.DependencyInjection;
using NeerCore.Typeids.Internal;

namespace NeerCore.Typeids.Api;

public class TypeidsProcessor : TypeidsProcessorBase
{
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

    private static object? ParseString(string? stringValue, Type targetType)
    {
        if (string.IsNullOrEmpty(stringValue)) return null;
        if (targetType == Int16Type) return short.Parse(stringValue);
        if (targetType == Int32Type) return int.Parse(stringValue);
        if (targetType == Int64Type) return long.Parse(stringValue);
        if (targetType == StringType) return stringValue;
        if (targetType == GuidType) return Guid.Parse(stringValue);
        return null;
    }
}