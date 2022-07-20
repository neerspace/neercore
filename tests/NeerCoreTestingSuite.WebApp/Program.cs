using Microsoft.EntityFrameworkCore;
using NeerCore.Api;
using NeerCore.Api.Extensions;
using NeerCore.Api.Extensions.Swagger;
using NeerCore.Application.Extensions;
using NeerCore.Data.EntityFramework;
using NeerCore.Mapping.Extensions;
using NeerCoreTestingSuite.WebApp;
using NeerCoreTestingSuite.WebApp.Data;
using NLog;

var logger = LoggerInstaller.InitDefault();

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
    builder.Services.AddDatabase<SqliteDbContext>(db =>
            db.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));

    builder.Services.AddMediatorApplicationFromCurrentAssembly();
    builder.Services.RegisterMappersFromCurrentAssembly();
    builder.AddNeerApi();
}

static WebApplication ConfigureWebApp(WebApplication app)
{
    if (app.Configuration.GetSwaggerSettings().Enabled)
        app.UseCustomSwagger();

    app.UseCors(CorsPolicies.AcceptAll);
    app.UseHttpsRedirection();

    app.UseCustomExceptionHandler();

    app.MapControllers();

    return app;
}