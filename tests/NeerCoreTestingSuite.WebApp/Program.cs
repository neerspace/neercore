using Microsoft.EntityFrameworkCore;
using NeerCore.Api;
using NeerCore.Api.Extensions;
using NeerCore.Api.Extensions.Swagger;
using NeerCore.Application.Extensions;
using NeerCore.Data.EntityFramework;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.Logging;
using NeerCore.Logging.Extensions;
using NeerCore.Mapping.Extensions;
using NeerCoreTestingSuite.WebApp.Data;
using NeerCoreTestingSuite.WebApp.Data.Entities;
using NLog;

var logger = LoggerInstaller.InitFromCurrentEnvironment();

var factory = new SqliteDbContextFactory();
factory.CreateDbContext(null);

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
    builder.Logging.AddNLogAsDefault();

    builder.Services.AddDatabase<SqliteDbContext>(db =>
        db.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));

    builder.Services.AddMediatorApplication();
    builder.Services.RegisterMappers();
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
    }

    if (app.Configuration.GetSwaggerSettings().Enabled)
        app.UseCustomSwagger();

    app.UseCors(CorsPolicies.AcceptAll);
    app.UseHttpsRedirection();

    app.UseCustomExceptionHandler();

    app.MapControllers();

    return app;
}