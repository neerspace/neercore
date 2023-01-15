using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NeerCore.Api.Swagger.Filters;
using NeerCore.Api.Swagger.Internal;
using NeerCore.Localization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeerCore.Api.Swagger;

internal sealed class SwaggerConfiguration : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IWebHostEnvironment _environment;
    private readonly SwaggerConfigurationOptions _config;
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly OpenApiInfoProviderOptions _apiInfoOptions;

    public SwaggerConfiguration(
        IWebHostEnvironment environment,
        IApiVersionDescriptionProvider provider,
        IOptions<SwaggerConfigurationOptions> swaggerOptionsAccessor,
        IOptions<OpenApiInfoProviderOptions> apiInfoSettingsAccessor)
    {
        _config = swaggerOptionsAccessor.Value;
        _environment = environment;
        _provider = provider;
        _apiInfoOptions = apiInfoSettingsAccessor.Value;

        _apiInfoOptions.ConfigureDelegate ??= _ => new OpenApiInfo();
    }

    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (var description in _provider.ApiVersionDescriptions)
            options.SwaggerDoc(description.GroupName.ToLower(), CreateVersionInfo(description));

        options.DocumentFilter<XLogoDocumentFilter>();
        options.DocumentFilter<JsonPatchDocumentFilter>();

        options.MapType(typeof(LocalizedString), () => new OpenApiSchema { Type = SwaggerSchemaTypes.String });

        if (_config.Security.Enabled)
        {
            options.OperationFilter<AuthorizeCheckOperationFilter>();

            options.AddSecurityDefinition(_config.Security.Scheme, new OpenApiSecurityScheme
            {
                Description = _config.Security.Description,
                Name = _config.Security.ParameterName,
                In = _config.Security.ParameterLocation,
                Type = _config.Security.SchemeType,
                BearerFormat = _config.Security.BearerFormat,
                Scheme = _config.Security.Scheme
            });
        }

        foreach (string xmlPath in GetXmlComments())
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }

    public void Configure(string name, SwaggerGenOptions options) => Configure(options);


    private OpenApiInfo CreateVersionInfo(ApiVersionDescription versionDescription)
    {
        string descriptionFilePath = Path.Join(_environment.ContentRootPath, _config.Description);
        string description = File.Exists(descriptionFilePath)
            ? File.ReadAllText(descriptionFilePath)
            : _config.Description ?? default!;

        if (!string.IsNullOrWhiteSpace(description))
            description = description.Replace("{version}", versionDescription.GroupName.ToLower());

        OpenApiInfo openApiInfo = _apiInfoOptions.ConfigureDelegate!.Invoke(versionDescription);
        openApiInfo.Version ??= versionDescription.ApiVersion.ToString();
        openApiInfo.Description ??= description;
        openApiInfo.Title ??= _config.Title;
        return openApiInfo;
    }

    private IEnumerable<string> GetXmlComments()
    {
        // Set the comments path for the Swagger JSON and UI
        if (_config.IncludeComments is not { Length: > 0 })
            yield break;

        foreach (string xmlDocsFile in _config.IncludeComments)
        {
            string xmlDocPath = Path.Combine(AppContext.BaseDirectory, xmlDocsFile);
            if (File.Exists(xmlDocPath))
                yield return xmlDocPath;
        }
    }
}