using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NeerCore.Api;
using NeerCore.Api.Extensions;
using NeerCore.Api.Extensions.Swagger;
using NeerCore.Application.Extensions;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.Data.EntityFramework.Extensions;
using NeerCore.DependencyInjection.Extensions;
using NeerCore.Logging;
using NeerCore.Logging.Extensions;
using NeerCore.Mapping.Extensions;
using NeerCoreTestingSuite.WebApp.Data;
using NeerCoreTestingSuite.WebApp.Data.Entities;
using NeerCoreTestingSuite.WebApp.Settings;
using NLog;

var logger = LoggerInstaller.InitFromCurrentEnvironment();

try
{
    var builder = WebApplication.CreateBuilder(args);
    ConfigureBuilder(builder);
    ConfigureWebApp(builder.Build()).Run();
}
catch (Exception e)
{
    logger.Fatal(e);
}
finally
{
    logger.Info("Application is now stopping");
    LogManager.Shutdown();
}

// ==========================================

static void ConfigureBuilder(WebApplicationBuilder builder)
{
    builder.Logging.ConfigureNLogAsDefault();

    builder.Services.AddDatabase<SqliteDbContext, SqliteDbContextFactory>();
    builder.Services.AddMediatorApplication();
    builder.Services.ConfigureAllOptions();
    builder.Services.AddAllMappers();

    builder.Services.AddNeerApiServices();
    builder.Services.AddNeerControllers();
}

static WebApplication ConfigureWebApp(WebApplication app)
{
    // Test SQL logging
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<IDatabaseContext>();
        var teas = db.Set<Tea>()
            .AsNoTracking()
            .Where(e => e.Price > 10)
            .Select(e => new { e.Id, e.Price, e.Name })
            .ToList();

        var settings = scope.ServiceProvider.GetRequiredService<IOptions<TestSettings>>();
        Console.WriteLine(settings.Value.Message);
    }

    if (app.Configuration.GetSwaggerSettings().Enabled)
        app.UseCustomSwagger();

    app.UseCors(CorsPolicies.AcceptAll);
    app.UseHttpsRedirection();

    app.UseNeerExceptionHandler();

    app.MapControllers();

    return app;
}