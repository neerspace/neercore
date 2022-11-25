using System.Reflection;
using NeerCore.DependencyInjection.Extensions;

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
    public static string ProjectRootNamespace { get; private set; } = Assembly.GetEntryAssembly()?.FullName?.Split('.')[0] ?? "";

    private static List<Assembly>? applicationAssemblies;
    private static List<Assembly>? allAssemblies;

    /// <summary>
    ///   Returns a list of types only from your assemblies.
    /// </summary>
    /// <remarks>
    ///   Please use naming style like 'MyApp.Application', 'MyApp.Data.Sqlite',
    ///   if you want to work with this method in correct way :)
    /// </remarks>
    public static IList<Assembly> ApplicationAssemblies => applicationAssemblies ??= AllAssemblies.Where(IsApplicationAssembly).ToList();

    /// <summary>
    ///   Returns a list of types from all available assemblies.
    /// </summary>
    public static IList<Assembly> AllAssemblies => allAssemblies ??= LoadAllAssemblies().ToList();

    /// <summary>
    ///   The way how to determinate that a assembly is source of your app.
    /// </summary>
    public static Func<Assembly, bool> IsApplicationAssembly { get; set; } = asm =>
        asm.FullName != null && asm.FullName.StartsWith(ProjectRootNamespace, StringComparison.OrdinalIgnoreCase);

    public static IEnumerable<Type> GetImplementationsFromAssembly<TBase>(Assembly assembly)
    {
        return GetImplementationsFromAssembly(typeof(TBase), assembly);
    }

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
    public static IEnumerable<Type> GetImplementationsFromAssembly(Type baseType, Assembly assembly)
    {
        if (assembly is null) throw new ArgumentNullException(nameof(assembly));
        return assembly.GetTypes()
            .Where(t => t.InheritsFrom(baseType));
    }

    /// <inheritdoc cref="GetImplementationsOf{TBase}"/>
    public static IEnumerable<Type> GetImplementationsOf(Type baseType, Func<Assembly, bool>? assemblySelector = null)
    {
        var assemblies = assemblySelector is null ? ApplicationAssemblies : AllAssemblies.Where(assemblySelector);
        return assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.InheritsFrom(baseType));
    }

    /// <summary>
    ///   Returns a list of all available assemblies in app.
    /// </summary>
    /// <returns>Assemblies sequence.</returns>
    public static IEnumerable<Assembly> LoadAllAssemblies(Assembly? rootAssembly = null)
    {
        var list = new List<string>();
        var stack = new Stack<Assembly>();

        stack.Push(rootAssembly ?? Assembly.GetEntryAssembly()!);

        do
        {
            Assembly asm = stack.Pop();
            yield return asm;

            var referenceAssemblies = asm.GetReferencedAssemblies()
                .Where(a => !list.Contains(a.FullName));
            foreach (AssemblyName reference in referenceAssemblies)
            {
                stack.Push(Assembly.Load(reference));
                list.Add(reference.FullName);
            }
        } while (stack.Count > 0);
    }

    public static Assembly? TryLoad(string assemblyName)
    {
        try
        {
            return Assembly.Load(assemblyName);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    public static void ConfigureRoot(Type rootType)
    {
        SetRootNamespace(rootType.Namespace!);
        AddAssembly(rootType.Assembly);
    }

    public static void AddAssembly(Assembly assembly)
    {
        if (assembly is null)
            throw new ArgumentNullException(nameof(assembly));
        var allAsm = (List<Assembly>)AllAssemblies;
        var appAsm = (List<Assembly>)ApplicationAssemblies;

        var assemblies = LoadAllAssemblies(assembly).ToArray();
        allAsm.AddRange(assemblies);
        appAsm.AddRange(assemblies.Where(IsApplicationAssembly));
    }

    public static void SetRootNamespace(string ns)
    {
        if (string.IsNullOrEmpty(ns))
            throw new ArgumentNullException(nameof(ns));
        ProjectRootNamespace = ns.Split('.')[0];
    }
}