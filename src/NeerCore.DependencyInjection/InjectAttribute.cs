using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.DependencyInjection;

/// <summary>
/// Attribute to simple reference your service class with DI
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class InjectAttribute : Attribute
{
	/// <summary>
	/// Specifies a DI scope lifetime
	/// </summary>
	public ServiceLifetime Lifetime { get; init; } = ServiceLifetime.Transient;

	/// <summary>
	/// Specify injection type
	/// </summary>
	public InjectionType InjectionType { get; init; } = InjectionType.Auto;

	/// <summary>
	/// Specify injection base type
	/// </summary>
	public Type? ServiceType { get; set; }
}