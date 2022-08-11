using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NeerCore.Tests;

public static partial class TestingEnvironment
{
    private static IConfiguration? cachedConfiguration;
    private static IHostEnvironment? cachedEnvironment;
    private static IServiceProvider? cachedServiceProvider;

    public static IMediator Mediator => TestServices.GetRequiredService<IMediator>();
    public static IServiceProvider TestServices => cachedServiceProvider ??= BuildServiceProvider(TestConfiguration, TestEnvironment);
    public static IConfiguration TestConfiguration => cachedConfiguration ??= BuildConfiguration();
    public static IHostEnvironment TestEnvironment => cachedEnvironment ??= BuildEnvironment();
}