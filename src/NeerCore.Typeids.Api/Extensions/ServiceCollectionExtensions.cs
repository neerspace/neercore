using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Typeids.Api.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeerCore.Typeids.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTypeids(this IServiceCollection services)
    {
        services.AddScoped<ITypeidsProcessor, TypeidsProcessor>();
        services.AddSingleton<ITypeidsProcessor, TypeidsProcessor>();

        services.Configure<SwaggerGenOptions>(swagger => swagger.AddTypeidsFilter());
        services.Configure<MvcOptions>(mvc => mvc.ModelBinderProviders.Insert(0, new TypeidsModelBinderProvider()));
        services.Configure<JsonOptions>(json => json.JsonSerializerOptions.Converters.Add(new TypeidsConverterFactory()));
        return services;
    }
}