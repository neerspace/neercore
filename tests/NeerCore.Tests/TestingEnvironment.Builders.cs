using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using NeerCore.Application.Extensions;
using NeerCore.DependencyInjection.Extensions;
using NeerCore.Mapping.Extensions;

namespace NeerCore.Tests;

public static partial class TestingEnvironment
{
    private static IServiceProvider BuildServiceProvider(IConfiguration configuration, IHostEnvironment environment)
    {
        var services = new ServiceCollection();
        services.AddSingleton(configuration);
        services.AddSingleton(environment);

        services.AddTestLogger();
        services.AddTestHttpContext();
        services.AddTestDatabase();
        services.AddMediatorApplication();
        services.ConfigureAllOptions();
        services.AddAllMappers();

        return services.BuildServiceProvider();
    }

    private static IConfiguration BuildConfiguration()
    {
        string configPath = Path.Join(Directory.GetCurrentDirectory(), "../../../appsettings.Testing.json");
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configPath, optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();
    }

    private static IWebHostEnvironment BuildEnvironment(string environmentName = "Testing")
    {
        return Mock.Of<IWebHostEnvironment>(env =>
            env.EnvironmentName == environmentName);
    }
}