namespace NeerCore.Api.Extensions.Swagger;

public class SwaggerConfigurationOptions
{
	public bool Enabled { get; init; }
	public string? Title { get; init; }
	public string? Description { get; init; }
	public string[]? IncludeComments { get; init; }
}