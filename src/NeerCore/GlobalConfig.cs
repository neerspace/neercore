using System.Reflection;
using NeerCore.Extensions;

namespace NeerCore;

public static class GlobalConfig
{
	private static Assembly? applicationRootAssembly;
	private static string? applicationNamespace;

	public static Assembly ApplicationRootAssembly
	{
		get => applicationRootAssembly ??= Assembly.GetExecutingAssembly();
		set => applicationRootAssembly = value;
	}

	public static string ApplicationNamespace => applicationNamespace ??= ApplicationRootAssembly.GetBaseNamespace();
}