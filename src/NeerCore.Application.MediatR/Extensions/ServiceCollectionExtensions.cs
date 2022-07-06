using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.Application.Extensions;

public static class ServiceCollectionExtensions
{
	/// <inheritdoc cref="AddMediatorApplication(IServiceCollection,IEnumerable{Assembly})"/>
	public static void AddMediatorApplication(this IServiceCollection services, string assemblyName)
	{
		services.AddMediatorApplication(new[] { assemblyName });
	}

	/// <inheritdoc cref="AddMediatorApplication(IServiceCollection,IEnumerable{Assembly})"/>
	public static void AddMediatorApplication(this IServiceCollection services, Assembly assembly)
	{
		services.AddMediatorApplication(new[] { assembly });
	}

	/// <inheritdoc cref="AddMediatorApplication(IServiceCollection,IEnumerable{Assembly})"/>
	public static void AddMediatorApplicationFromCurrentAssembly(this IServiceCollection services)
	{
		services.AddMediatorApplication(new[] { Assembly.GetCallingAssembly() });
	}

	/// <inheritdoc cref="AddMediatorApplication(IServiceCollection,IEnumerable{Assembly})"/>
	public static void AddMediatorApplication(this IServiceCollection services, IEnumerable<string> assemblyNames)
	{
		var assemblies = assemblyNames.Select(asm => AppDomain.CurrentDomain.Load(asm)).ToArray();
		services.AddMediatorApplication(assemblies);
	}

	/// <summary>Adds MediatR with embedded FluentValidation.</summary>
	/// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
	/// <param name="assemblies">Mediator <see cref="IRequest"/> and <see cref="IRequestHandler{TRequest}"/> implementations assemblies.</param>
	public static void AddMediatorApplication(this IServiceCollection services, IEnumerable<Assembly> assemblies)
	{
		var assembliesArray = assemblies as Assembly[] ?? assemblies.ToArray();

		services.AddMediatR(assembliesArray).AddFluentValidation(options =>
		{
			options.DisableDataAnnotationsValidation = true;
		});
		services.AddFluentValidation(assembliesArray);
	}
}