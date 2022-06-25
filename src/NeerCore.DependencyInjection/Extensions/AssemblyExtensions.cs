using System.Reflection;

namespace NeerCore.DependencyInjection.Extensions;

public static class AssemblyExtensions
{
	/// <summary>Returns a root namespace part.</summary>
	/// <param name="assembly">An instance of any assembly.</param>
	/// <exception cref="ArgumentNullException"><paramref name="assembly" /> is <see langword="null" />.</exception>
	/// <returns>For assembly with namespace name 'NeerCore.Data.Abstractions' returns a 'NeerCore'.</returns>
	public static string GetBaseNamespace(this Assembly assembly)
	{
		if (assembly is null) throw new ArgumentNullException(nameof(assembly));
		return assembly.GetName().Name!.Split('.')[0];
	}
}