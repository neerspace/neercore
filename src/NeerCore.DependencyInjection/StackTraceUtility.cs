using System.Diagnostics;
using System.Reflection;

namespace NeerCore.DependencyInjection;

public static class StackTraceUtility
{
	private static readonly Assembly neerAssembly = typeof(StackTraceUtility).Assembly;
	private static readonly Assembly mscorlibAssembly = typeof(string).Assembly;
	private static readonly Assembly systemAssembly = typeof(Debug).Assembly;


	/// <summary>
	///		Returns the Assembly from which the method that will call this method was called.
	///		Or throws an <see cref="NullReferenceException"/> if caller assembly is <see langword="null"/>.
	/// </summary>
	/// <exception cref="NullReferenceException">If caller assembly is <see langword="null"/>.</exception>
	/// <inheritdoc cref="GetCallerAssembly"/>
	public static Assembly GetRequiredCallerAssembly(int framesToSkip = 2)
	{
		return GetCallerAssembly(framesToSkip) ?? throw new NullReferenceException("Caller assembly not found.");
	}


	/// <summary>Returns the Assembly from which the method that will call this method was called.</summary>
	/// <param name="framesToSkip">Stack trace calls to skip.</param>
	/// <returns>Assembly from which the method that will call this method was called.</returns>
	public static Assembly? GetCallerAssembly(int framesToSkip = 2)
	{
		var stackFrame = new StackFrame(framesToSkip, false);

		var method = stackFrame.GetMethod();
		if (method is null) return null;

		var assembly = method.DeclaringType?.Assembly ?? method.Module.Assembly;

		if (assembly == neerAssembly
		    || assembly == mscorlibAssembly
		    || assembly == systemAssembly)
			return null;

		return assembly;
	}
}