using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NeerCore.DependencyInjection.Extensions;
using NeerCore.DependencyInjection.TestsSideAssembly;
using NeerCore1.DependencyInjection.TestsSideAssembly;

namespace NeerCore.DependencyInjection.Tests.Tests.Extensions;

public sealed class ServiceCollectionExtensionsOptionsTests
{
    private readonly IConfiguration _configuration;

    public ServiceCollectionExtensionsOptionsTests()
    {
        AssemblyProvider.ConfigureRoot(GetType());

        var configMock = new Mock<IConfiguration>();
        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.SetupGet(c => c.Value).Returns(() => "Test");
        configMock.Setup(c => c.GetSection("Test")).Returns(() => configSectionMock.Object);
        _configuration = configMock.Object;
    }

    [Fact]
    public void ConfigureAllOptionsTest()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_configuration);
        services.ConfigureAllOptions();
        var provider = services.BuildServiceProvider();

        string? opts = provider.GetService<IOptions<TestOptionRoot>>()?.Value?.Sample;
        Assert.NotNull(opts);

        opts = provider.GetService<IOptions<TestOption>>()?.Value?.Sample;
        Assert.NotNull(opts);

        opts = provider.GetService<IOptions<TestOption1>>()?.Value?.Sample;
        Assert.NotNull(opts);
    }

    [Fact]
    public void ConfigureAllOptionsFromAssemblyTest()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_configuration);
        services.ConfigureAllOptionsFromAssembly(GetType().Assembly);
        var provider = services.BuildServiceProvider();

        string? opts = provider.GetService<IOptions<TestOptionRoot>>()?.Value?.Sample;
        Assert.NotNull(opts);

        opts = provider.GetService<IOptions<TestOption>>()?.Value?.Sample;
        Assert.Null(opts);

        opts = provider.GetService<IOptions<TestOption1>>()?.Value?.Sample;
        Assert.Null(opts);
    }
}