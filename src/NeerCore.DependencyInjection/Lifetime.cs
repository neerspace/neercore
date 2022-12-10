using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.DependencyInjection;

/// <summary>
///   Specifies the lifetime of a service in an <see cref="IServiceCollection"/>.
///   Extended analogue for <see cref="Microsoft.Extensions.DependencyInjection.ServiceLifetime"/>.
/// </summary>
[Flags]
public enum Lifetime : byte
{
    /// <summary>
    ///   Uses the default value defined at configuring injection.
    /// </summary>
    Default = 0x1,

    /// <summary>
    ///   Specifies that a single instance of the service will be created.
    /// </summary>
    Singleton = 0x2,

    /// <summary>
    ///   Specifies that a new instance of the service will be created for each scope.
    /// </summary>
    /// <remarks>
    ///   In ASP.NET Core applications a scope is created around each server request.
    /// </remarks>
    Scoped = 0x4,

    /// <summary>
    ///   Specifies that a new instance of the service will be created every time it is requested.
    /// </summary>
    Transient = 0x8
}