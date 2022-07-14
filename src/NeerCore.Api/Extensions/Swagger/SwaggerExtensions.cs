using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.Api.Extensions.Swagger;

public static class SwaggerExtensions
{
    public static void AddCustomSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.ConfigureOptions<SwaggerConfiguration>();
        services.AddSwaggerGen();
    }

    public static void UseCustomSwagger(this IApplicationBuilder app)
    {
        var swaggerSettings = app.ApplicationServices.GetRequiredService<IConfiguration>().GetSwaggerSettings();
        var apiProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        if (swaggerSettings.Enabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in apiProvider.ApiVersionDescriptions)
                {
                    string name = $"{swaggerSettings.Title} {description.GroupName.ToUpper()}";
                    string url = $"/swagger/{description.GroupName}/swagger.json";
                    options.SwaggerEndpoint(url, name);
                }

                options.RoutePrefix = swaggerSettings.SwaggerUrl;
                options.DocumentTitle = swaggerSettings.Title;
                options.InjectStylesheet("/swagger/custom.css");
                options.InjectJavascript("/swagger/custom.js");
            });
        }

        if (swaggerSettings.ApiDocs)
        {
            app.UseReDoc(options =>
            {
                var description = apiProvider.ApiVersionDescriptions[0];
                options.DocumentTitle = $"{swaggerSettings.Title} {description.GroupName.ToUpper()}";
                options.SpecUrl = $"../swagger/{description.GroupName}/swagger.json";
                options.RoutePrefix = swaggerSettings.ApiDocsUrl.Replace("{version}", description.GroupName.ToLower());
                options.HeadContent = swaggerSettings.ApiDocsHeadContent;
            });

            foreach (var description in apiProvider.ApiVersionDescriptions)
            {
                app.UseReDoc(options =>
                {
                    options.DocumentTitle = $"{swaggerSettings.Title} {description.GroupName.ToUpper()}";
                    options.SpecUrl = $"../swagger/{description.GroupName}/swagger.json";
                    options.RoutePrefix = swaggerSettings.ApiDocsUrl.Replace("{version}", description.GroupName.ToLower());
                    options.HeadContent = swaggerSettings.ApiDocsHeadContent;
                });
            }
        }
    }

    public static SwaggerConfigurationOptions GetSwaggerSettings(this IConfiguration configuration)
    {
        var settings = new SwaggerConfigurationOptions();
        configuration.Bind("Swagger", settings);
        return settings;
    }
}