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
		app.UseSwagger();
		app.UseSwaggerUI(options =>
		{
			var swaggerSettings = app.ApplicationServices.GetRequiredService<IConfiguration>().GetSwaggerSettings();
			var apiProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

			foreach (ApiVersionDescription description in apiProvider.ApiVersionDescriptions)
			{
				string name = $"{swaggerSettings.Title} {description.GroupName.ToUpper()}";
				string url = $"/swagger/{description.GroupName}/swagger.json";
				options.SwaggerEndpoint(url, name);
			}

			options.DocumentTitle = swaggerSettings.Title;
			options.InjectStylesheet("/swagger/custom.css");
			options.InjectJavascript("/swagger/custom.js");
		});
	}

	public static SwaggerConfigurationOptions GetSwaggerSettings(this IConfiguration configuration)
	{
		var settings = new SwaggerConfigurationOptions();
		configuration.Bind("Swagger", settings);
		return settings;
	}
}