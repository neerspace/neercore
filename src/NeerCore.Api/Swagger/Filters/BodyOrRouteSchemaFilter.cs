using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeerCore.Api.Swagger.Filters;

/// <summary>
///
/// </summary>
public class BodyOrRouteSchemaFilter : ISchemaFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties is not null && schema.Properties.Count > 0)
        {
            var routeProps = context.Type.GetProperties()
                .Where(p => p.GetCustomAttribute<FromRouteAttribute>() != null)
                .ToArray();

            foreach (var routeProperty in routeProps)
            {
                var propertyToRemove = schema.Properties.FirstOrDefault(sp =>
                    sp.Key.Equals(routeProperty.Name, StringComparison.OrdinalIgnoreCase)).Key;
                if (propertyToRemove is not null)
                    schema.Properties.Remove(propertyToRemove);
            }
        }
    }
}