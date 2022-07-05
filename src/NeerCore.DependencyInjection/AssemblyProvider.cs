using System.Reflection;

namespace NeerCore.DependencyInjection;

// TODO: documentation

/// <summary>
///	  DO NOT OPEN THIS CLASS!!!
///	  TOO MUCH REFLECTION HERE!!!
/// <br/>
/// <remarks>
///	  Please use naming style like 'MyApp.Application', 'MyApp.Data.Sqlite',
///	  if you want to work with this class in correct way :)
/// </remarks>
/// </summary>
public static class AssemblyProvider
{
	private static readonly string ProjectRootName = Assembly.GetExecutingAssembly().GetName().FullName.Split('.')[0];
	private static IEnumerable<Assembly>? applicationAssemblies;

	/// <summary>
	/// 
	/// </summary>
	public static IEnumerable<Assembly> ApplicationAssemblies => applicationAssemblies ??= LoadAllAssemblies().Where(IsApplicationAssembly);

	public static readonly Func<Assembly, bool> IsApplicationAssembly = asm =>
			asm.FullName != null && asm.FullName.StartsWith(ProjectRootName, StringComparison.OrdinalIgnoreCase);

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TBase"></typeparam>
	/// <returns></returns>
	public static IEnumerable<Type> GetImplementationsOf<TBase>()
	{
		Type baseType = typeof(TBase);

		return ApplicationAssemblies.SelectMany(a => a.GetTypes())
				.Where(t => t != baseType && baseType.IsAssignableFrom(t))
				.ToList();
	}

	/// <summary>Returns a list of all available assemblies in app.</summary>
	/// <returns>Assemblies sequence.</returns>
	public static IEnumerable<Assembly> LoadAllAssemblies()
	{
		var list = new List<string>();
		var stack = new Stack<Assembly>();

		stack.Push(Assembly.GetEntryAssembly()!);

		do
		{
			Assembly asm = stack.Pop();
			yield return asm;

			var referenceAssemblies = asm.GetReferencedAssemblies().Where(a => !list.Contains(a.FullName));
			foreach (AssemblyName reference in referenceAssemblies)
			{
				stack.Push(Assembly.Load(reference));
				list.Add(reference.FullName);
			}
		} while (stack.Count > 0);
	}
}