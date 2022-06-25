using System.Globalization;
using JuniorTemplate.Application;
using JuniorTemplate.Data;
using JuniorTemplate.Infrastructure;
using NeerCore;
using NeerCore.Api;
using NeerCore.Api.Extensions;
using NeerCore.Api.Extensions.Swagger;
using NeerCore.DependencyInjection;
using NLog;

GlobalConfig.ApplicationRootAssembly = typeof(Program).Assembly;
CultureInfo.CurrentCulture = new CultureInfo("en");
var logger = LoggerInstaller.InitDefault();

try
{
	var builder = WebApplication.CreateBuilder(args);
	ConfigureBuilder(builder);

	var app = builder.Build();
	ConfigureWebApp(app);

	app.Run();
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
	builder.Services.AddSqlServerDatabase();
	builder.Services.AddApplication(builder.Configuration);
	builder.Services.AddInfrastructure();

	builder.AddNeerApi();
}

static void ConfigureWebApp(WebApplication app)
{
	if (app.Configuration.GetSwaggerSettings().Enabled)
		app.UseCustomSwagger();

	app.UseCors(CorsPolicies.AcceptAll);
	app.UseHttpsRedirection();

	app.UseCustomExceptionHandler();

	app.MapControllers();
}