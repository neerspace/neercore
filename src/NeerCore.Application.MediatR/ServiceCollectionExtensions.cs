using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static void AddMediatRApplication(this IServiceCollection services, params string[] assemblyNames)
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