using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.DependencyInjection;

/// <summary>
///   Service injection configuration options.
/// </summary>
public sealed record InjectionOptions
{
    /// <summary>
    ///   A list of <see cref="Assembly"/> where stored services marked with <see cref="ServiceAttribute"/>.
    /// </summary>
    public IEnumerable<Assembly>? ServiceAssemblies { get; set; }

    /// <summary>
    ///   The default lifetime for all services where the `Lifetime` is not manually overridden.
    /// </summary>
    public ServiceLifetime DefaultLifetime { get; set; } = ServiceLifetime.Transient;

    /// <summary>
    ///   The default injection mode for all services where the `InjectionType` is not manually overridden.
    /// </summary>
    public InjectionType DefaultInjectionType { get; set; } = InjectionType.Auto;

    /// <summary>
    ///   Allows (true) or disallows (false) the resolver to inject
    ///   the implementations with internal visibility.
    /// </summary>
    public bool ResolveInternalImplementations { get; set; } = false;

    /// <summary>
    ///   Overrides global application environment with a value you define
    ///   <b>(only when registering services)</b>.
    /// </summary>
    public string? Environment { get; set; }
}