using Microsoft.Extensions.Options;

namespace NeerCore.DependencyInjection.Tests;

public class TestOptionRoot
{
    public string? Sample { get; set; }
}

public class TestOptionConfigurator : IConfigureOptions<TestOptionRoot>
{
    public void Configure(TestOptionRoot options)
    {
        options.Sample = "Test";
    }
}