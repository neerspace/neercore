using Microsoft.AspNetCore.JsonPatch;
using Microsoft.OpenApi.Models;
using NeerCore.Localization;

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
    ///   Enables or disables ReDoc
    /// </summary>
    public bool ExtendedDocs { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    // public bool RestResponses { get; set; } = false;

    /// <summary>
    ///   Enables or disables nullable reference types
    /// </summary>
    public bool NullableRefTypesSupport { get; set; } = true;

    /// <summary>
    ///   Enables or disables custom models for <see cref="JsonPatchDocument"/>
    /// </summary>
    public bool JsonPatchSupport { get; set; } = true;

    /// <summary>
    ///   Enables or disables support for NeerCore <see cref="LocalizedString"/>
    /// </summary>
    public bool LocalizedStringSupport { get; set; } = true;

    /// <summary>
    ///   Base swagger docs URL
    /// </summary>
    public string SwaggerUrl { get; set; } = "swagger";

    /// <summary>
    /// Allowed values: <c>["json", "yaml"]</c>.
    /// </summary>
    public string[]? OpenapiFormats { get; set; }

    /// <summary>
    ///   Base URL for ReDoc
    /// </summary>
    public string ApiDocsUrl { get; set; } = "docs-{version}";

    /// <summary>
    ///   ReDoc docs header content
    /// </summary>
    public string ApiDocsHeadContent { get; set; } = "";

    /// <summary>
    ///   Swagger built-in security options
    /// </summary>
    public SwaggerSecurityOptions Security { get; set; } = new();

    /// <summary>
    ///   Custom Swagger Docs title 
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///   Path to the text or markdown file with app documentation to display it in Swagger 
    /// </summary>
    // public string? DescriptionFilePath { get; set; }

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