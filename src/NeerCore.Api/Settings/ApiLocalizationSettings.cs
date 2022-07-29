namespace NeerCore.Api.Settings;

[Obsolete]
public class ApiLocalizationSettings
{
    public string DefaultCulture { get; init; } = default!;
    public string[]? SupportedCultures { get; init; }
}