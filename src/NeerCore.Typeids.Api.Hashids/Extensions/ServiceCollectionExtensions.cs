using AspNetCore.Hashids.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Typeids.Api.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeerCore.Typeids.Api.Hashids.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTypeidsWithHashids(this IServiceCollection services, Action<HashidsOptions>? setup = null)
    {
        services.AddHashids(setup ?? (_ => { }));
        services.AddScoped<ITypeidsProcessor, TypeidsHashidsProcessor>();
        services.AddSingleton<ITypeidsProcessor, TypeidsHashidsProcessor>();

        services.Configure<SwaggerGenOptions>(swagger => swagger.AddTypeidsFilter());
        services.Configure<MvcOptions>(mvc => mvc.ModelBinderProviders.Insert(0, new TypeidsModelBinderProvider()));
        services.Configure<JsonOptions>(json => json.JsonSerializerOptions.Converters.Add(new TypeidsHashidsConverterFactory()));

        return services;
    }
}