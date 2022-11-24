using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Api.Defaults.Middleware;
using NeerCore.DependencyInjection;
using ExceptionHandlerOptions = NeerCore.Api.Options.ExceptionHandlerOptions;

namespace NeerCore.Api.Extensions;

public static class MiddlewareExtensions
{
    /// <summary>
    ///   Adds all factory style middlewares that implements <see cref="IMiddleware"/> interface.
    /// </summary>
    /// <param name="services">A collection of service descriptors.</param>
    /// <param name="lifetime">Lifetime of a service in <see cref="services"/> <see cref="IServiceCollection"/>.</param>
    public static IServiceCollection AddFactoryMiddlewares(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.AddOptions<ExceptionHandlerOptions>();
        
        IEnumerable<Type> middlewares = AssemblyProvider.GetImplementationsOf<IMiddleware>(asm =>
            AssemblyProvider.IsApplicationAssembly(asm) || asm.GetName().Name!.StartsWith("NeerCore"));
        foreach (Type middleware in middlewares)
            services.Add(ServiceDescriptor.Describe(middleware, middleware, lifetime));

        return services;
    }

    /// <summary>
    ///   Adds <see cref="NeerExceptionHandlerMiddleware"/> to the application's request pipeline.
    /// </summary>
    /// <param name="app">An <see cref="ApplicationBuilder"/> instance.</param>
    public static IApplicationBuilder UseNeerExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<NeerExceptionHandlerMiddleware>();
    }
}