using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NeerCore.Extensions;
using Newtonsoft.Json.Serialization;
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
            .Where(item => item.Key.Equals(nameof(Operation)) || item.Key.Equals(nameof(IContractResolver)));
        foreach (var jsonPatchDocSchema in jsonPatchDocSchemas)
            swaggerDoc.Components.Schemas.Remove(jsonPatchDocSchema.Key);

        // Add correct 'Operation' schema instead of default
        swaggerDoc.Components.Schemas.Add(nameof(Operation), OperationSchema);

        // Fix '*JsonPatchDocument' schemas
        jsonPatchDocSchemas = swaggerDoc.Components.Schemas
            .Where(item => item.Key.Contains(nameof(JsonPatchDocument)));
        foreach (var jsonPatchDocSchema in jsonPatchDocSchemas)
        {
            string baseName = jsonPatchDocSchema.Key.Replace(nameof(JsonPatchDocument), "");
            var schema = jsonPatchDocSchema.Value;
            schema.Properties = new Dictionary<string, OpenApiSchema>
            {
                ["operations"] = new()
                {
                    Type = "array",
                    Items = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Id = baseName + nameof(Operation),
                            Type = ReferenceType.Schema,
                        }
                    }
                }
            };
            if (schema.Description.IsNullOrEmpty())
                schema.Description = "Array of operations to perform";
        }
    }

    private static OpenApiSchema OperationSchema => new()
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            { "op", new OpenApiSchema { Type = "string", Enum = OperationNameEnum } },
            { "path", new OpenApiSchema { Type = "string", Example = new OpenApiString("/path/to/property") } },
            { "value", new OpenApiSchema { Type = "string", Example = new OpenApiString("new value") } },
        }
    };

    private static List<IOpenApiAny> OperationNameEnum => new()
    {
        new OpenApiString("add"),
        new OpenApiString("copy"),
        new OpenApiString("move"),
        new OpenApiString("remove"),
        new OpenApiString("replace"),
        new OpenApiString("test"),
    };
}