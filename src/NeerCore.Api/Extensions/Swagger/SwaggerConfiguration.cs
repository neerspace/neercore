using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NeerCore.Api.DocumentFilters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeerCore.Api.Extensions.Swagger;

public class SwaggerConfiguration : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IWebHostEnvironment _environment;
    private readonly SwaggerConfigurationOptions _options;
    private readonly IApiVersionDescriptionProvider _provider;

    public SwaggerConfiguration(IConfiguration configuration, IWebHostEnvironment environment, IApiVersionDescriptionProvider provider)
    {
        _options = configuration.GetSwaggerSettings();
        _environment = environment;
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
            options.SwaggerDoc(description.GroupName.ToLower(), CreateVersionInfo(description));

        options.DocumentFilter<XLogoDocumentFilter>();
        options.DocumentFilter<JsonPatchDocumentFilter>();

        foreach (string xmlPath in GetXmlComments())
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }


    public void Configure(string name, SwaggerGenOptions options) => Configure(options);


    private OpenApiInfo CreateVersionInfo(ApiVersionDescription versionDescription)
    {
        string descriptionFilePath = Path.Join(_environment.ContentRootPath, _options.Description);
        string description = File.Exists(descriptionFilePath)
                ? File.ReadAllText(descriptionFilePath)
                : _options.Description ?? default!;

        description = description.Replace("{version}", versionDescription.GroupName.ToLower());

        return new OpenApiInfo
        {
            Title = _options.Title,
            Version = versionDescription.ApiVersion.ToString(),
            Description = description,
        };
    }

    private IEnumerable<string> GetXmlComments()
    {
        // Set the comments path for the Swagger JSON and UI
        if (_options.IncludeComments is not { Length: > 0 })
            yield break;

        foreach (string xmlDocsFile in _options.IncludeComments)
        {
            string xmlDocPath = Path.Combine(AppContext.BaseDirectory, xmlDocsFile);
            if (File.Exists(xmlDocPath))
                yield return xmlDocPath;
        }
    }
}