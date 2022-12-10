using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace NeerCore1.DependencyInjection.TestsSideAssembly;

public class TestOption1
{
    public string? Sample { get; set; }
}

public class TestOptionConfigurator : IConfigureOptions<TestOption1>
{
    private readonly IConfiguration _configuration;

    public TestOptionConfigurator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(TestOption1 options)
    {
        options.Sample = _configuration.GetSection("Test")?.Value;
    }
}