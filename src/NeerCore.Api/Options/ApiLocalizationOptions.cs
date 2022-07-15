namespace NeerCore.Api.Options;

public class ApiLocalizationOptions
{
    public string DefaultCulture { get; init; } = default!;
    public string[]? SupportedCultures { get; init; }
}