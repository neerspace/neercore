using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeerCore.Api.Swagger.Filters;

public sealed class AuthorizeCheckOperationFilter : IOperationFilter
{
    private readonly SwaggerSecurityOptions _securityOptions;

    public AuthorizeCheckOperationFilter(IOptions<SwaggerConfigurationOptions> optionsAccessor)
    {
        _securityOptions = optionsAccessor.Value.Security;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        context.ApiDescription.TryGetMethodInfo(out var methodInfo);

        if (methodInfo == null)
            return;

        var hasAuthorizeAttribute = false;

        if (methodInfo.MemberType == MemberTypes.Method)
        {
            // NOTE: Check the controller itself has Authorize attribute
            hasAuthorizeAttribute = methodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ?? false;

            // NOTE: Controller has Authorize attribute, so check the endpoint itself.
            //       Take into account the allow anonymous attribute
            if (hasAuthorizeAttribute)
                hasAuthorizeAttribute = !methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
            else
                hasAuthorizeAttribute = methodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
        }

        if (!hasAuthorizeAttribute)
            return;

        if (operation.Responses.All(r => r.Key != StatusCodes.Status401Unauthorized.ToString()))
            operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new OpenApiResponse { Description = "Unauthorized" });
        if (operation.Responses.All(r => r.Key != StatusCodes.Status403Forbidden.ToString()))
            operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new OpenApiResponse { Description = "Forbidden" });

        // NOTE: This adds the "Padlock" icon to the endpoint in swagger, 
        //       we can also pass through the names of the policies in the string[]
        //       which will indicate which permission you require.
        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = _securityOptions.Scheme
                        },
                        Scheme = _securityOptions.Scheme,
                        Name = _securityOptions.Scheme,
                        In = _securityOptions.ParameterLocation
                    },
                    new List<string>()
                }
            }
        };
    }
}