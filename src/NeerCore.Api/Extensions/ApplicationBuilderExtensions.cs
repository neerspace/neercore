using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.DependencyInjection;

namespace NeerCore.Api.Extensions;

public static class ApplicationBuilderExtensions
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
}