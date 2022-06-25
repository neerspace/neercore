using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.DependencyInjection;

namespace NeerCore.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static void AddMediatorApplication(this IServiceCollection services, string assemblyName)
	{
		services.AddMediatorApplication(new[] { assemblyName });
	}

	public static void AddMediatorApplication(this IServiceCollection services, Assembly assembly)
	{
		services.AddMediatorApplication(new[] { assembly });
	}

	public static void AddMediatorApplicationFromCurrentAssembly(this IServiceCollection services)
	{
		services.AddMediatorApplication(new[] { StackTraceUtility.GetRequiredCallerAssembly() });
	}

	public static void AddMediatorApplication(this IServiceCollection services, IEnumerable<string> assemblyNames)
	{
		var assemblies = assemblyNames.Select(asm => AppDomain.CurrentDomain.Load(asm)).ToArray();
		services.AddMediatorApplication(assemblies);
	}

	public static void AddMediatorApplication(this IServiceCollection services, IEnumerable<Assembly> assemblies)
	{
		var assembliesArray = assemblies as Assembly[] ?? assemblies.ToArray();
		services.AddMediatR(assembliesArray).AddFluentValidation(ConfigureFluentValidation);
		services.AddFluentValidation(assembliesArray);
	}

	public static void ConfigureFluentValidation(FluentValidationMvcConfiguration options)
	{
		options.DisableDataAnnotationsValidation = true;
		options.ImplicitlyValidateChildProperties = true;
		options.ImplicitlyValidateRootCollectionElements = true;
	}
}