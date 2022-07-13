using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.DependencyInjection;

public record InjectionOptions
{
	public IEnumerable<Assembly>? ServiceAssemblies { get; set; }
	public ServiceLifetime DefaultLifetime { get; set; } = ServiceLifetime.Transient;
	public InjectionType DefaultInjectionType { get; set; } = InjectionType.Auto;
}