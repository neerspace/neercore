using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using NeerCore.Api.Swagger.Extensions;
using NeerCore.DependencyInjection.Extensions;
using NeerCore.Localization;

namespace NeerCore.Api.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///   Adds default NeerCore stuff into your application services.
    /// </summary>
    /// <param name="services">The services available in the application.</param>
    public static IServiceCollection AddNeerApiServices(this IServiceCollection services)
    {
        return services.AddNeerApiServices(Assembly.GetCallingAssembly());
    }

    public static IServiceCollection AddNeerApiServices(this IServiceCollection services, params string[] assemblyNames)
    {
        return services.AddNeerApiServices(assemblyNames.Select(Assembly.Load).ToArray());
    }

    public static IServiceCollection AddNeerApiServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        return services.AddNeerApiServices(assemblies, configureInfo: null);
    }

    public static IServiceCollection AddNeerApiServices(
        this IServiceCollection services, IEnumerable<Assembly> assemblies, Func<ApiVersionDescription, OpenApiInfo>? configureInfo)
    {
        services.AddServicesFromAssemblies(assemblies);

        services.AddFactoryMiddlewares();
        services.AddDefaultCorsPolicy();
        services.AddNeerApiVersioning();
        services.ConfigureApiBehaviorOptions();
        services.AddNeerSwagger(configureInfo);

        return services;
    }


    /// <summary>
    ///   Adds services for controllers to the specified <see cref="IServiceCollection"/>
    ///   with configured conventions. For example there is <see cref="KebabCaseNamingConvention"/>
    ///   used out of a box and <see cref="JsonStringEnumConverter"/> too.
    /// </summary>
    /// <param name="services">The services available in the application.</param>
    public static IMvcBuilder AddNeerControllers(this IServiceCollection services)
    {
        return services.AddControllers(KebabCaseNamingConvention.Use)
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new LocalizedStringJsonConverter());
            });
    }
}