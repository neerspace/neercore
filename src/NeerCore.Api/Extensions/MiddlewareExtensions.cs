using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Api.Defaults.Middleware;
using NeerCore.DependencyInjection;

namespace NeerCore.Api.Extensions;

public static class MiddlewareExtensions
{
	public static void AddFactoryMiddlewares(this IServiceCollection services)
	{
		IEnumerable<Type> middlewares = AssemblyProvider.GetImplementationsOf<IMiddleware>();
		foreach (Type middleware in middlewares)
			services.AddScoped(middleware);
	}

	public static void UseCustomExceptionHandler(this IApplicationBuilder app)
	{
		app.UseMiddleware<ExceptionHandlerMiddleware>();
	}
}