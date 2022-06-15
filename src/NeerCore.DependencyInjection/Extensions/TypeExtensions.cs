using System.Reflection;

namespace NeerCore.DependencyInjection.Extensions;

public static class TypeExtensions
{
	/// <summary>
	/// Simple way to get attribute on type.
	/// </summary>
	/// <param name="type">The type from which to get the attribute</param>
	/// <returns>Attribute or null</returns>
	public static TAttribute? GetAttribute<TAttribute>(this Type type)
	{
		Attribute? attr = type.GetCustomAttribute(typeof(TAttribute));
		return attr is null ? default : (TAttribute) (object) attr;
	}

	/// <summary>
	/// Simple way to get attribute on type.
	/// </summary>
	/// <param name="type">The type from which to get the attribute</param>
	/// <returns>Attribute or throws exception</returns>
	/// <exception cref="TypeLoadException">Throws if TAttribute not found on type</exception>
	public static TAttribute GetRequiredAttribute<TAttribute>(this Type type)
	{
		Attribute? attr = type.GetCustomAttribute(typeof(TAttribute));
		if (attr is null)
			throw new TypeLoadException($"Attribute {typeof(TAttribute).Name} for type {type.Name} not found.");

		return (TAttribute) (object) attr;
	}
}