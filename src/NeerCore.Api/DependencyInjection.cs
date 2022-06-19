using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NeerCore.Api.Extensions;
using NeerCore.DependencyInjection;
using NLog;
using NLog.Web;

namespace NeerCore.Api;

public static class DependencyInjection
{
	public static void AddNeerApi(this WebApplicationBuilder builder) =>
			builder.AddNeerApi(StackTraceUtility.GetCallerAssembly());

	public static void AddNeerApi(this WebApplicationBuilder builder, params string[] assemblyNames) =>
			builder.AddNeerApi(assemblyNames.Select(Assembly.Load).ToArray());

	public static void AddNeerApi(this WebApplicationBuilder builder, params Assembly[] assemblies)
	{
		builder.Logging.AddNLog();
		builder.Services.AddNeerApiServices(assemblies);

		builder.Services.AddControllers(KebabCaseNamingConvention.Use);
	}

	/// <summary>
	/// Changes default logging provider to NLog
	/// </summary>
	public static void AddNLog(this ILoggingBuilder logging)
	{
		logging.ClearProviders();
		logging.AddNLogWeb(LogManager.Configuration);
	}
}