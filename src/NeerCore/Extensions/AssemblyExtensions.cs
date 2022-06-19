using System.Reflection;

namespace NeerCore.Extensions;

public static class AssemblyExtensions
{
	public static string GetBaseNamespace(this Assembly assembly)
	{
		return assembly.GetName().Name!.Split('.')[0];
	}
}