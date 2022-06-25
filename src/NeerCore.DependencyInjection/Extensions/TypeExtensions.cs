using System.Reflection;

namespace NeerCore.DependencyInjection.Extensions;

public static class TypeExtensions
{
	/// <summary>Simple way to get attribute on type.</summary>
	/// <param name="type">The type from which to get the attribute</param>
	/// <returns><see cref="TAttribute"/> or <see langword="null"/>.</returns>
	public static TAttribute? GetAttribute<TAttribute>(this Type type)
	{
		Attribute? attr = type.GetCustomAttribute(typeof(TAttribute));
		return attr is null ? default : (TAttribute) (object) attr;
	}

	/// <summary>Simple way to get attribute on type.</summary>
	/// <param name="type">The type from which to get the attribute</param>
	/// <returns><see cref="TAttribute"/> or throws <see cref="TypeLoadException"/>.</returns>
	/// <exception cref="TypeLoadException">If TAttribute not found on type</exception>
	public static TAttribute GetRequiredAttribute<TAttribute>(this Type type)
	{
		Attribute? attr = type.GetCustomAttribute(typeof(TAttribute));
		if (attr is null)
			throw new TypeLoadException($"Attribute {typeof(TAttribute).Name} for type {type.Name} not found.");

		return (TAttribute) (object) attr;
	}
}