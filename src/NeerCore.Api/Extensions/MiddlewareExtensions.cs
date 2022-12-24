using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NeerCore.Api.Middleware;
using NeerCore.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

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

    /// <summary>
    ///   Register the NeerSwaggerUI middleware as custom alternative for default SwaggerUI middleware
    /// </summary>
    /// <param name="app">An <see cref="ApplicationBuilder"/> instance.</param>
    /// <param name="options"></param>
    public static IApplicationBuilder UseNeerSwaggerUI(this IApplicationBuilder app, SwaggerUIOptions options)
    {
        return app.UseMiddleware<NeerSwaggerUIMiddleware>(options);
    }

    /// <summary>
    ///   Register the NeerSwaggerUI middleware as custom alternative for default SwaggerUI middleware
    /// </summary>
    /// <param name="app">An <see cref="ApplicationBuilder"/> instance.</param>
    /// <param name="configureAction"></param>
    public static IApplicationBuilder UseNeerSwaggerUI(this IApplicationBuilder app, Action<SwaggerUIOptions>? configureAction = null)
    {
        SwaggerUIOptions options;
        using (var scope = app.ApplicationServices.CreateScope())
        {
            options = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<SwaggerUIOptions>>().Value;
            configureAction?.Invoke(options);
        }

        // To simplify the common case, use a default that will work with the SwaggerMiddleware defaults
        if (options.ConfigObject.Urls == null)
        {
            var hostingEnv = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            options.ConfigObject.Urls = new[]
            {
                new UrlDescriptor
                {
                    Name = $"{hostingEnv.ApplicationName} v1",
                    Url = "v1/swagger.json"
                }
            };
        }

        return app.UseNeerSwaggerUI(options);
    }
}