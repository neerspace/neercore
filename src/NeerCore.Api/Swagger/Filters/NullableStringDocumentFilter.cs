using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeerCore.Api.Swagger.Filters;

/// <summary>
///
/// </summary>
public class NullableStringDocumentFilter : IDocumentFilter
{
    private static readonly Type s_stringType = typeof(string);
    private const string NullableAttribute = "System.Runtime.CompilerServices.NullableAttribute";

    /// <inheritdoc cref="IDocumentFilter.Apply"/>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var nullableProperties = context.ApiDescriptions.SelectMany(d => d.ParameterDescriptions)
            .SelectMany(d => d.Type.GetProperties()
                .Where(p => p.PropertyType == s_stringType
                    && p.CustomAttributes.Any(at =>
                        string.Equals(at.AttributeType.FullName, NullableAttribute, StringComparison.Ordinal)))).ToArray();
        var processingTypes = nullableProperties.Select(np => np.DeclaringType!).Distinct().ToArray();

        foreach (var (schemaName, schema) in swaggerDoc.Components.Schemas)
        {
            if (!processingTypes.Any(p => string.Equals(schemaName, p.Name, StringComparison.Ordinal)))
                continue;

            foreach (var (propName, propSchema) in schema.Properties)
            {
                if (!nullableProperties.Any(p => string.Equals(p.Name, propName, StringComparison.OrdinalIgnoreCase)))
                    propSchema.Nullable = false;
            }
        }
    }
}