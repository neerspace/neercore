namespace NeerCore.Api.Extensions.Swagger;

public sealed class SwaggerConfigurationOptions
{
    public bool Enabled { get; init; } = true;
    public bool ApiDocs { get; init; } = true;
    public bool ExtendedDocs { get; init; } = false;
    public string SwaggerUrl { get; init; } = "swagger";
    public string ApiDocsUrl { get; init; } = "docs-{version}";
    public string ApiDocsHeadContent { get; init; } = "";
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string[]? IncludeComments { get; init; }
}