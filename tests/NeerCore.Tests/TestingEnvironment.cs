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

    public static IMediator Mediator => ServiceProvider.GetRequiredService<IMediator>();
    public static IServiceProvider ServiceProvider => cachedServiceProvider ??= BuildServiceProvider(Configuration, Environment);
    public static IConfiguration Configuration => cachedConfiguration ??= BuildConfiguration();
    public static IHostEnvironment Environment => cachedEnvironment ??= BuildEnvironment();
}