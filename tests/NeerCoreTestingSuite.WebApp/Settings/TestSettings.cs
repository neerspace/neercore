using Microsoft.Extensions.Options;

namespace NeerCoreTestingSuite.WebApp.Settings;

public class TestSettings
{
    public string Message { get; set; } = default!;
}

public class ConfigureTestSettings : IConfigureOptions<TestSettings>
{
    private readonly IConfiguration _configuration;
    public ConfigureTestSettings(IConfiguration configuration) => _configuration = configuration;

    public void Configure(TestSettings options)
    {
        options.Message = $"Hello {_configuration.GetValue<string>("Test")}!";
    }
}