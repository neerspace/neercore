using Microsoft.Extensions.DependencyInjection;
using NeerCore.DependencyInjection.Extensions;
using NeerCore.DependencyInjection.TestsSideAssembly;
using NeerCore1.DependencyInjection.TestsSideAssembly;

namespace NeerCore.DependencyInjection.Tests.Tests.Extensions;

public sealed class ServiceCollectionExtensionsServicesTests
{
    public ServiceCollectionExtensionsServicesTests()
    {
        AssemblyProvider.ConfigureRoot(GetType());
    }

    [Fact]
    public void AddAllServicesTest()
    {
        var services = new ServiceCollection();
        services.AddAllServices(options =>
        {
            options.DefaultLifetime = ServiceLifetime.Singleton;
            options.ResolveInternalImplementations = true;
        });
        var provider = services.BuildServiceProvider();

        bool? result = provider.GetService<IServiceRoot>()?.Test();
        Assert.True(result);

        result = provider.GetService<ServiceRoot>()?.Test();
        Assert.Null(result);

        result = provider.GetService<ServiceBase>()?.Test();
        Assert.True(result);

        result = provider.GetService<ServiceX>()?.Test();
        Assert.Null(result);

        result = provider.GetService<Service1>()?.Test();
        Assert.True(result);
    }
}