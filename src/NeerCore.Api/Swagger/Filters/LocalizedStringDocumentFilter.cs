using Microsoft.OpenApi.Models;
using NeerCore.Api.Swagger.Internal;
using NeerCore.Localization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeerCore.Api.Swagger.Filters;

/// <summary>
///
/// </summary>
public sealed class LocalizedStringDocumentFilter : IDocumentFilter
{
    /// <inheritdoc cref="IDocumentFilter.Apply"/>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Components.Schemas.Remove(nameof(LocalizedValue));

        swaggerDoc.Components.Schemas.Add("LocalizedString", new OpenApiSchema
        {
            Type = SwaggerSchemaTypes.String
        });
    }
}