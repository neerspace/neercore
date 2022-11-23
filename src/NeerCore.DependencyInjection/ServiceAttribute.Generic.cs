namespace NeerCore.DependencyInjection;

/// <summary>
///   Attribute to simple reference your service class with DI.
/// </summary>
/// <remarks>
///   Generic overload supported only in .NET 7
/// </remarks>
public class ServiceAttribute<T> : ServiceAttribute
{
    /// <inheritdoc cref="ServiceType"/>
    public sealed override Type? ServiceType { get; set; }

    public ServiceAttribute()
    {
        ServiceType = typeof(T);
    }
}