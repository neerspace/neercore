using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static void AddMediatorApplication(this IServiceCollection services, string assemblyName)
	{
		services.AddMediatorApplication(new[] { assemblyName });
	}

	public static void AddMediatorApplication(this IServiceCollection services, IEnumerable<string> assemblyNames)
	{
		var assemblies = assemblyNames.Select(asm => AppDomain.CurrentDomain.Load(asm)).ToArray();

		services.AddMediatR(assemblies).AddFluentValidation(ConfigureFluentValidation);
		services.AddFluentValidation(assemblies);
	}

	public static void ConfigureFluentValidation(FluentValidationMvcConfiguration options)
	{
		options.DisableDataAnnotationsValidation = true;
		options.ImplicitlyValidateChildProperties = true;
		options.ImplicitlyValidateRootCollectionElements = true;
	}
}