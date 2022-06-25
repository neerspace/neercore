using System.Reflection;
using NeerCore.DependencyInjection.Extensions;

namespace NeerCore.DependencyInjection;

/// <summary>Global assembly provider configuration.</summary>
public static class GlobalConfig
{
	private static Assembly? applicationRootAssembly;
	private static string? applicationNamespace;

	/// <summary>Gets and sets the application root assembly.</summary>
	public static Assembly ApplicationRootAssembly
	{
		get => applicationRootAssembly ??= Assembly.GetExecutingAssembly();
		set => applicationRootAssembly = value;
	}

	/// <summary>Gets a root namespace name of <see cref="ApplicationRootAssembly"/>.</summary>
	public static string ApplicationNamespace => applicationNamespace ??= ApplicationRootAssembly.GetBaseNamespace();
}