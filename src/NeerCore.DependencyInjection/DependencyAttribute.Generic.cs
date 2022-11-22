#if NET7_0_OR_GREATER

namespace NeerCore.DependencyInjection;

/// <summary>
///   Attribute to simple reference your service class with DI.
/// </summary>
/// <remarks>
///   Generic overload supported only in .NET 7
/// </remarks>
public class DependencyAttribute<T> : DependencyAttribute
{
    /// <inheritdoc cref="ServiceType"/>
    public sealed override Type? ServiceType { get; set; }

    public DependencyAttribute()
    {
        ServiceType = typeof(T);
    }
}

#endif