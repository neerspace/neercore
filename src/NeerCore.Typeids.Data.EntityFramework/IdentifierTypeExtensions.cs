namespace NeerCore.Typeids.Data.EntityFramework;

internal static class IdentifierTypeExtensions
{
    public static Type GetIdentifierValueType(this Type typeToConvert)
    {
        return typeToConvert.GetInterfaces()
                   .FirstOrDefault(it => it.IsGenericType && it.Name.StartsWith("ITypeIdentifier"))?
                   .GetGenericArguments().FirstOrDefault()
               ?? throw new ArgumentException($"'{typeToConvert.Name}' is not implemented interface 'ITypeIdentifier<TValue>'");
    }
}