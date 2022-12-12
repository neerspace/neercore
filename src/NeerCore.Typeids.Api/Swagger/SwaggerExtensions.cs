using Microsoft.Extensions.DependencyInjection;
using NeerCore.Typeids.Api.Swagger.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeerCore.Typeids.Api.Swagger;

/// <summary>
///   Typeids Swagger integration extensions class.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    ///   Adds <see cref="TypeidsDocumentFilter"/> to swagger. 
    /// </summary>
    /// <param name="options">Swagger gen options</param>
    public static SwaggerGenOptions AddTypeidsFilter(this SwaggerGenOptions options)
    {
        options.DocumentFilter<TypeidsDocumentFilter>();
        return options;
    }
}