using System.Reflection;

namespace NeerCore.DependencyInjection;

/// <summary>
///   Wrapper to simplify usage of some reflection features.
/// </summary>
/// <remarks>
///   Please use naming style like 'MyApp.Application', 'MyApp.Data.Sqlite',
///   if you want to work with this class in correct way :)
/// </remarks>
public static class AssemblyProvider
{
    private static readonly string ProjectRootName = Assembly.GetEntryAssembly()?.FullName?.Split('.')[0] ?? "";
    private static IEnumerable<Assembly>? applicationAssemblies;
    private static IEnumerable<Assembly>? allAssemblies;

    /// <summary>
    ///   Returns a list of types only from your assemblies.
    /// </summary>
    /// <remarks>
    ///   Please use naming style like 'MyApp.Application', 'MyApp.Data.Sqlite',
    ///   if you want to work with this method in correct way :)
    /// </remarks>
    public static IEnumerable<Assembly> ApplicationAssemblies => applicationAssemblies ??= AllAssemblies.Where(IsApplicationAssembly);

    /// <summary>
    ///   Returns a list of types from all available assemblies.
    /// </summary>
    public static IEnumerable<Assembly> AllAssemblies => allAssemblies ??= LoadAllAssemblies();

    /// <summary>The way how to determinate that a assembly is source of your app.</summary>
    public static readonly Func<Assembly, bool> IsApplicationAssembly = asm =>
        asm.FullName != null && asm.FullName.StartsWith(ProjectRootName, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    ///   Returns a list of all <typeparamref name="TBase"/> implementations
    ///   (and child classes) that available in app assemblies.
    /// </summary>
    /// <param name="assemblySelector">
    ///   Filters specific assemblies to search implementations there
    ///   (if null only application assemblies will be researched).
    /// </param>
    /// <typeparam name="TBase">Parent class or interface.</typeparam>
    /// <returns>A list of found types.</returns>
    public static IEnumerable<Type> GetImplementationsOf<TBase>(Func<Assembly, bool>? assemblySelector = null)
    {
        return GetImplementationsOf(typeof(TBase), assemblySelector);
    }

    /// <inheritdoc cref="GetImplementationsOf{TBase}"/>
    public static IEnumerable<Type> GetImplementationsOf(Type baseType, Func<Assembly, bool>? assemblySelector = null)
    {
        var assemblies = assemblySelector is null ? ApplicationAssemblies : AllAssemblies.Where(assemblySelector);
        return assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.InheritsFrom(baseType));
    }

    /// <summary>
    ///   Safely checks is the <paramref name="t1"/> is inherited from <paramref name="t2"/>.
    /// </summary>
    /// <returns><b>true</b> is inherits otherwise <b>false</b></returns>
    public static bool InheritsFrom(this Type? t1, Type? t2)
    {
        if (t1 is null || t2 is null)
            return false;
        if (t1.BaseType is { IsGenericType: true } && t1.BaseType.GetGenericTypeDefinition() == t2)
            return true;
        if (InheritsFrom(t1.BaseType!, t2))
            return true;

        return (t2.IsAssignableFrom(t1) && t1 != t2) ||
               t1.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == t2);
    }

    /// <summary>
    ///   Returns a list of all available assemblies in app.
    /// </summary>
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