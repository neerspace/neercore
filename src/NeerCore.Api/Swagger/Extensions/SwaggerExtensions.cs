using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NeerCore.Api.Extensions;

namespace NeerCore.Api.Swagger.Extensions;

public static class SwaggerExtensions
{
    private const string SwaggerConfigurationSectionName = "Swagger";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureInfo"></param>
    public static IServiceCollection AddNeerSwagger(this IServiceCollection services, Func<ApiVersionDescription, OpenApiInfo>? configureInfo = null)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        services.AddEndpointsApiExplorer();
        services.Configure<SwaggerConfigurationOptions>(options => configuration.Bind(SwaggerConfigurationSectionName, options));
        services.Configure<OpenApiInfoProviderOptions>(options => options.ConfigureDelegate = configureInfo);
        services.ConfigureOptions<SwaggerConfiguration>();
        services.AddSwaggerGen();

        return services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    public static IApplicationBuilder UseNeerSwagger(this IApplicationBuilder app)
    {
        var swaggerOptions = app.ApplicationServices.GetRequiredService<IOptions<SwaggerConfigurationOptions>>().Value;
        var apiProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        if (swaggerOptions.Enabled)
        {
            app.UseSwagger();
            app.UseNeerSwaggerUI(options =>
            {
                foreach (var description in apiProvider.ApiVersionDescriptions)
                {
                    var name = $"{swaggerOptions.Title} {description.GroupName.ToUpper()}";
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    options.SwaggerEndpoint(url, name);
                }

                options.RoutePrefix = swaggerOptions.SwaggerUrl;
                options.DocumentTitle = swaggerOptions.Title;
                options.EnableFilter();

                if (swaggerOptions.ExtendedDocs)
                {
                    options.InjectStylesheet("/neercore/swagger-extensions.css");
                    options.InjectJavascript("/neercore/swagger-extensions.js");
                }
            });
        }

        if (swaggerOptions.ApiDocs)
        {
            foreach (var description in apiProvider.ApiVersionDescriptions)
            {
                app.UseReDoc(options =>
                {
                    options.DocumentTitle = $"{swaggerOptions.Title} {description.GroupName.ToUpper()}";
                    options.SpecUrl = $"../swagger/{description.GroupName}/swagger.json";
                    options.RoutePrefix = swaggerOptions.ApiDocsUrl.Replace("{version}", description.GroupName.ToLower());
                    options.HeadContent = swaggerOptions.ApiDocsHeadContent;
                });
            }
        }

        return app;
    }

    public static SwaggerConfigurationOptions GetSwaggerSettings(this IConfiguration configuration)
    {
        var settings = new SwaggerConfigurationOptions();
        configuration.Bind(SwaggerConfigurationSectionName, settings);
        return settings;
    }
}