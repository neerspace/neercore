﻿using System.Reflection;

namespace NeerCore.Globals;

public static class GlobalConfiguration
{
	private static Assembly? applicationRootAssembly;
	private static string? applicationNamespace;

	public static Assembly ApplicationRootAssembly
	{
		get => applicationRootAssembly ??= Assembly.GetExecutingAssembly();
		set => applicationRootAssembly = value;
	}

	public static string ApplicationNamespace => applicationNamespace ??= ApplicationRootAssembly.GetName().Name!.Split('.')[0];
}