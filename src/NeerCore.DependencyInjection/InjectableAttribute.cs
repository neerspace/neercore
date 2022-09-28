namespace NeerCore.DependencyInjection;

/// <summary>
///   Attribute to simple reference your service class with DI.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
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
    ///   <b>If not null</b> - Registers a service ONLY FOR specified environment.
    ///   In all others, this service will not be available!
    /// </summary>
    public string? Environment { get; set; }

    /// <summary>
    ///   <b>If true</b> - Registers a service ONLY FOR the 'Production' environment.
    ///   In all others, this service will not be available!
    /// </summary>
    public bool ProductionOnly
    {
        set => Environment = "Production";
    }

    /// <summary>
    ///   <b>If true</b> - Registers a service ONLY FOR the 'Development' environment.
    ///   In all others, this service will not be available!
    /// </summary>
    public bool DevelopmentOnly
    {
        set => Environment = "Development";
    }
}