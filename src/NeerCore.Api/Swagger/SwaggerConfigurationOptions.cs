using Microsoft.OpenApi.Models;

namespace NeerCore.Api.Swagger;

/// <summary>
///
/// </summary>
public sealed class SwaggerConfigurationOptions
{
    /// <summary>
    ///   Disables if <b>false</b> (enabled by default)
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    public bool ApiDocs { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    public bool ExtendedDocs { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    public bool RestResponses { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    public bool NullableRefTypesSupport { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    public bool JsonPatchSupport { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    public bool LocalizedStringSupport { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    public string SwaggerUrl { get; set; } = "swagger";

    /// <summary>
    ///
    /// </summary>
    public string ApiDocsUrl { get; set; } = "docs-{version}";

    /// <summary>
    ///
    /// </summary>
    public string ApiDocsHeadContent { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    public SwaggerSecurityOptions Security { get; set; } = new();

    /// <summary>
    ///
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? DescriptionFilePath { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string[]? IncludeComments { get; set; }
}

/// <summary>
///
/// </summary>
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