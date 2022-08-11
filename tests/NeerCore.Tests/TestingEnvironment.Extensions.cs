using HttpContextMoq;
using HttpContextMoq.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NeerCore.Data.EntityFramework.Extensions;
using NeerCoreTestingSuite.WebApp.Data;

namespace NeerCore.Tests;

public static partial class TestingEnvironment
{
    private static void AddTestHttpContext(this IServiceCollection services)
    {
        var httpContext = new HttpContextMock()
            .SetupUrl("https://neercore.net")
            .Mock.Object;
        services.AddSingleton(httpContext);
    }

    private static void AddTestLogger(this IServiceCollection services)
    {
        services.AddScoped(typeof(ILogger<>), typeof(Logger<>));
        services.AddSingleton<ILoggerFactory, LoggerFactory>();
    }

    private static void AddTestDatabase(this IServiceCollection services)
    {
        services.AddDatabase<SqliteDbContext>(optionsBuilder =>
            optionsBuilder.UseInMemoryDatabase("MemoDb_test" + DateTime.Now.ToFileTimeUtc()));
    }
}