using System.Globalization;
using NeerCore;
using NeerCore.Api;
using NeerCore.Api.Extensions;
using NeerCore.Api.Extensions.Swagger;
using NLog;
using SeniorTemplate.Application;
using SeniorTemplate.Data;
using SeniorTemplate.Infrastructure;

CultureInfo.CurrentCulture = new CultureInfo("en");
GlobalConfig.ApplicationRootAssembly = typeof(Program).Assembly;
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
	builder.Services.AddSqlServerDatabase(builder.Configuration);
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