using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NeerCore.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeerCore.Api.DocumentFilters;

/// <summary>
///   Swagger document filter to hide redundant models from
///   <see cref="Microsoft.AspNetCore.JsonPatch.JsonPatchDocument{TModel}"/>.
/// </summary>
public class JsonPatchDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // Remove default JsonPatchDocument schemas
        var jsonPatchDocSchemas = swaggerDoc.Components.Schemas
            .Where(item => item.Key.Equals("Operation") || item.Key.Equals("IContractResolver"));
        foreach (var jsonPatchDocSchema in jsonPatchDocSchemas)
            swaggerDoc.Components.Schemas.Remove(jsonPatchDocSchema.Key);

        // Add correct 'Operation' schema instead of default
        swaggerDoc.Components.Schemas.Add("Operation", new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>
            {
                {
                    "op", new OpenApiSchema
                    {
                        Type = "string", Enum = new List<IOpenApiAny>
                        {
                            new OpenApiString("add"),
                            new OpenApiString("copy"),
                            new OpenApiString("move"),
                            new OpenApiString("remove"),
                            new OpenApiString("replace"),
                            new OpenApiString("test"),
                        }
                    }
                },
                { "path", new OpenApiSchema { Type = "string", Example = new OpenApiString("/path/to/property") } },
                { "value", new OpenApiSchema { Type = "string", Example = new OpenApiString("new value") } },
            }
        });

        // Fix '*JsonPatchDocument' schemas
        jsonPatchDocSchemas = swaggerDoc.Components.Schemas
            .Where(item => item.Key.Contains("JsonPatchDocument"));
        foreach (var jsonPatchDocSchema in jsonPatchDocSchemas)
        {
            var schema = jsonPatchDocSchema.Value;
            schema.Properties = new Dictionary<string, OpenApiSchema>
            {
                ["operations"] = new()
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Operation",
                        Type = ReferenceType.Schema,
                    }
                }
            };
            if (schema.Description.IsNullOrEmpty())
                schema.Description = "Array of operations to perform";
        }
    }
}