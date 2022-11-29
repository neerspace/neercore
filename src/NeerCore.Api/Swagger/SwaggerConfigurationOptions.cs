using Microsoft.OpenApi.Models;

namespace NeerCore.Api.Swagger;

public sealed class SwaggerConfigurationOptions
{
    public bool Enabled { get; set; } = true;
    public bool ApiDocs { get; set; } = true;
    public bool ExtendedDocs { get; set; } = false;
    public bool RestResponses { get; set; } = false;
    public string SwaggerUrl { get; set; } = "swagger";
    public string ApiDocsUrl { get; set; } = "docs-{version}";
    public string ApiDocsHeadContent { get; set; } = "";
    public SwaggerSecurityOptions Security { get; set; } = new();
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string[]? IncludeComments { get; set; }
}

public sealed class SwaggerSecurityOptions
{
    public bool Enabled { get; set; } = false;
    public string Scheme { get; set; } = "Bearer";
    public SecuritySchemeType SchemeType { get; set; } = SecuritySchemeType.ApiKey;
    public string BearerFormat { get; set; } = "JWT";
    public string ParameterName { get; set; } = "Authorization";
    public ParameterLocation ParameterLocation { get; set; } = ParameterLocation.Header;
    public string Description { get; set; } = "";
}