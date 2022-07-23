using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Api.Extensions.Swagger;
using NeerCore.DependencyInjection.Extensions;
using Sieve.Services;

namespace NeerCore.Api.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///   
    /// </summary>
    /// <param name="services"></param>
    public static void AddNeerApiServices(this IServiceCollection services)
    {
        services.AddNeerApiServices(Assembly.GetCallingAssembly());
    }

    public static void AddNeerApiServices(this IServiceCollection services, params string[] assemblyNames)
    {
        services.AddNeerApiServices(assemblyNames.Select(Assembly.Load).ToArray());
    }

    public static void AddNeerApiServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddScoped<ISieveProcessor, SieveProcessor>();
        services.AddServicesFromAssemblies(assemblies);

        services.AddFactoryMiddlewares();
        services.AddDefaultCorsPolicy();
        services.AddCustomApiVersioning();
        services.ConfigureApiBehaviorOptions();
        services.AddCustomSwagger();
    }


    /// <summary>
    ///   Adds services for controllers to the specified <see cref="IServiceCollection"/>
    ///   with configured conventions. For example there is <see cref="KebabCaseNamingConvention"/>
    ///   used out of a box and <see cref="JsonStringEnumConverter"/> too.
    /// </summary>
    /// <param name="services"></param>
    public static void AddNeerControllers(this IServiceCollection services)
    {
        services.AddControllers(KebabCaseNamingConvention.Use)
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    }
}