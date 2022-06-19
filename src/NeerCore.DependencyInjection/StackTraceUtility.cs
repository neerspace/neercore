using System.Diagnostics;
using System.Reflection;

namespace NeerCore.DependencyInjection;

public static class StackTraceUtility
{
	private static readonly Assembly neerAssembly = typeof(StackTraceUtility).Assembly;
	private static readonly Assembly mscorlibAssembly = typeof(string).Assembly;
	private static readonly Assembly systemAssembly = typeof(Debug).Assembly;


	public static Assembly? GetCallerAssembly()
	{
		const int framesToSkip = 2;
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