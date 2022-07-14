using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.DependencyInjection;

/// <summary>
///   Service injection configuration options.
/// </summary>
public record InjectionOptions
{
    /// <summary>
    ///   A list of <see cref="Assembly"/> where stored services marked with <see cref="InjectableAttribute"/>.
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
}