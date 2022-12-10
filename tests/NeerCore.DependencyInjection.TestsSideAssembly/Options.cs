using Microsoft.Extensions.Options;

namespace NeerCore.DependencyInjection.TestsSideAssembly;

public class TestOption
{
    public string? Sample { get; set; }
}

public class TestOptionConfigurator : IConfigureOptions<TestOption>
{
    public void Configure(TestOption options)
    {
        options.Sample = "Test";
    }
}