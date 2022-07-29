using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.Api.Extensions;

public static class ApiExtensions
{
    public static void AddDefaultCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(o => o.AddPolicy(CorsPolicies.AcceptAll, builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));
    }

    /// <summary>
    ///   Adds service API versioning to the specified services collection.
    /// </summary>
    /// <param name="services">The services available in the application.</param>
    /// <param name="apiVersionParameterSource">Configures the source for defining API version parameters.</param>
    public static void AddCustomApiVersioning(this IServiceCollection services, IApiVersionReader? apiVersionParameterSource = null)
    {
        services.AddApiVersioning();

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
            options.AssumeDefaultVersionWhenUnspecified = true;

            if (apiVersionParameterSource is not null)
                options.ApiVersionParameterSource = apiVersionParameterSource;

            options.DefaultApiVersion = ApiVersion.Default;
        });
    }

    public static void ConfigureApiBehaviorOptions(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            // Disable default ModelState validation (because we use only FluentValidation)
            options.SuppressModelStateInvalidFilter = true;
        });
    }
}