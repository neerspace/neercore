using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.DependencyInjection;

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

    /// <summary>
    ///   Adds MediatR with embedded FluentValidation.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    public static void AddMediatorApplication(this IServiceCollection services)
    {
        services.AddMediatorApplication(AssemblyProvider.ApplicationAssemblies);
    }

    /// <inheritdoc cref="AddMediatorApplication(IServiceCollection,IEnumerable{Assembly})"/>
    public static void AddMediatorApplication(this IServiceCollection services, IEnumerable<string> assemblyNames)
    {
        var assemblies = assemblyNames.Select(Assembly.Load).ToArray();
        services.AddMediatorApplication(assemblies);
    }

    /// <summary>
    ///   Adds MediatR with embedded FluentValidation.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="assemblies">Mediator <see cref="IRequest"/> and <see cref="IRequestHandler{TRequest}"/> implementations assemblies.</param>
    public static void AddMediatorApplication(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        var assembliesArray = assemblies as Assembly[] ?? assemblies.ToArray();
        services.AddMediatR(assembliesArray).AddFluentValidationAutoValidation(options =>
        {
            // Disable default validation
            options.DisableDataAnnotationsValidation = true;
        }).AddFluentValidationClientsideAdapters();
        services.AddFluentValidation(assembliesArray);
    }
}