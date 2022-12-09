using System.Reflection;

namespace NeerCore.DependencyInjection.Extensions;

public static class TypeExtensions
{
    private static readonly Type ObjectType = typeof(object);

    /// <summary>
    ///   Simple way to get attribute on type.
    /// </summary>
    /// <param name="type">The type from which to get the attribute</param>
    /// <param name="searchInParents">Allows (if true) to search for given attribute in all parent classes.</param>
    /// <returns><see cref="TAttribute"/> or <see langword="null"/>.</returns>
    public static TAttribute? GetAttribute<TAttribute>(this Type type, bool searchInParents = false)
        where TAttribute : Attribute
    {
        Attribute? attr = type.GetCustomAttribute(typeof(TAttribute));
        if (attr is null && searchInParents)
            return TryGetAttributeFromParentClass<TAttribute>(type);

        return attr as TAttribute;
    }

    /// <summary>
    ///   Simple way to get required attribute on type.
    /// </summary>
    /// <param name="type">The type from which to get the attribute</param>
    /// <param name="searchInParents">Allows (if true) to search for given attribute in all parent classes.</param>
    /// <typeparam name="TAttribute">Attribute type to get.</typeparam>
    /// <returns><see cref="TAttribute"/> or throws <see cref="TypeLoadException"/>.</returns>
    public static TAttribute GetRequiredAttribute<TAttribute>(this Type type, bool searchInParents = false)
        where TAttribute : Attribute
    {
        var attr = type.GetAttribute<TAttribute>();
        if (attr is null && searchInParents)
            attr = TryGetAttributeFromParentClass<TAttribute>(type);

        return attr ?? throw new TypeLoadException($"Attribute '{typeof(TAttribute).Name}' for type {type.Name} not found.");
    }

    /// <summary>
    ///   Safely checks is the <paramref name="t1"/> is inherited from <paramref name="t2"/>.
    /// </summary>
    /// <returns><b>true</b> is inherits otherwise <b>false</b></returns>
    public static bool InheritsFrom(this Type? t1, Type? t2)
    {
        if (t1 is null || t2 is null)
            return false;
        if (t1.BaseType is { IsGenericType: true } && t1.BaseType.GetGenericTypeDefinition() == t2)
            return true;
        if (InheritsFrom(t1.BaseType!, t2))
            return true;

        return (t2.IsAssignableFrom(t1) && t1 != t2) ||
               t1.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == t2);
    }

    private static TAttribute? TryGetAttributeFromParentClass<TAttribute>(Type type) where TAttribute : Attribute
    {
        var attrType = typeof(TAttribute);
        while (type.BaseType is not null && type.BaseType != ObjectType)
        {
            Attribute? attr = type.BaseType.GetCustomAttribute(attrType);
            if (attr is null) type = type.BaseType;
            else return attr as TAttribute;
        }

        return null;
    }
}