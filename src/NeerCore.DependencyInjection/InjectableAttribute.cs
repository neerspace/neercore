using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.DependencyInjection;

[Obsolete("User NeerCore.DependencyInjection.InjectableAttribute instead of this.")]
public class InjectAttribute : InjectableAttribute
{
    /// <summary>
    ///   Specifies a DI scope lifetime.
    /// </summary>
    public new ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;

    /// <summary>
    ///   Specify injection type.
    /// </summary>
    public new InjectionType InjectionType { get; set; } = InjectionType.Auto;
}

/// <summary>
///   Attribute to simple reference your service class with DI.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class InjectableAttribute : Attribute
{
    /// <summary>
    ///   Specifies a DI scope lifetime.
    /// </summary>
    public InstanceLifetime Lifetime { get; set; } = InstanceLifetime.Default;

    /// <summary>
    ///   Specify injection type.
    /// </summary>
    public InjectionType InjectionType { get; set; } = InjectionType.Default;

    /// <summary>
    ///   Manually specifies injection type for current implementation.
    /// </summary>
    public Type? ServiceType { get; set; }

    /// <summary>
    ///   
    /// </summary>
    public string? Environment { get; set; }

    /// <summary>
    ///   
    /// </summary>
    public bool ProductionOnly
    {
        set => Environment = "Production";
    }

    /// <summary>
    ///   
    /// </summary>
    public bool DevelopmentOnly
    {
        set => Environment = "Development";
    }
}