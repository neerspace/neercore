using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NeerCore.DependencyInjection;
using NeerCore.Typeids.Abstractions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeerCore.Typeids.Api.Swagger.Filters;

public sealed class TypeidsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var identifierNames = AssemblyProvider.GetImplementationsOf(typeof(ITypeIdentifier<>))
            .Select(it => it.Name).ToArray();
        var identifierSchemas = swaggerDoc.Components.Schemas
            .Where(s => identifierNames.Any(name => name.Equals(s.Key, StringComparison.OrdinalIgnoreCase)));
        foreach (var identifierSchema in identifierSchemas)
        {
            var identifierValueSchema = identifierSchema.Value.Properties.First(prop =>
                prop.Key.Equals("Value", StringComparison.OrdinalIgnoreCase)).Value;

            var referencedSchemaProps = swaggerDoc.Components.Schemas
                .SelectMany(s => s.Value.Properties)
                .Where(prop => prop.Value.Reference?.Id == identifierSchema.Key);

            foreach (var referencedSchemaProp in referencedSchemaProps)
            {
                referencedSchemaProp.Value.Reference = null;

                if (identifierValueSchema.Type == "integer")
                {
                    referencedSchemaProp.Value.Type = "string";
                    referencedSchemaProp.Value.Example = new OpenApiString("A1B2C3");
                }
                else
                {
                    referencedSchemaProp.Value.Type = identifierValueSchema.Type;
                    referencedSchemaProp.Value.Format = identifierValueSchema.Format;
                }
            }

            swaggerDoc.Components.Schemas.Remove(identifierSchema.Key);
        }
    }
}