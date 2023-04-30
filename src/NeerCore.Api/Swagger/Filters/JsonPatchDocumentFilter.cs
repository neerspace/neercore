using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NeerCore.Api.Swagger.Internal;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using OperationType = Microsoft.AspNetCore.JsonPatch.Operations.OperationType;

namespace NeerCore.Api.Swagger.Filters;

/// <summary>
///   Swagger document filter to hide redundant models from
///   <see cref="Microsoft.AspNetCore.JsonPatch.JsonPatchDocument{TModel}"/>.
/// </summary>
public sealed class JsonPatchDocumentFilter : IDocumentFilter
{
    /// <inheritdoc cref="IDocumentFilter.Apply"/>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // Remove default JsonPatchDocument schemas
        RemoveContractResolverFromSchema(swaggerDoc);

        // Add correct 'Operation' schema instead of default
        FixOperationSchemas(swaggerDoc);

        // Apply correct '*JsonPatchDocument' schemas
        FixJsonPatchDocumentSchemas(swaggerDoc);
    }

    private static void RemoveContractResolverFromSchema(OpenApiDocument swaggerDoc)
    {
        if (swaggerDoc.Components.Schemas.ContainsKey(nameof(IContractResolver)))
            swaggerDoc.Components.Schemas.Remove(nameof(IContractResolver));
    }

    private static void FixOperationSchemas(OpenApiDocument swaggerDoc)
    {
        swaggerDoc.Components.Schemas.Remove(nameof(OperationType));
        var operationSchemas = swaggerDoc.Components.Schemas
            .Select(x => x.Key)
            .Where(key => key.EndsWith(nameof(Operation)));
        foreach (var operationName in operationSchemas)
            swaggerDoc.Components.Schemas.Remove(operationName);

        swaggerDoc.Components.Schemas.Add("PatchOperation", new OpenApiSchema
        {
            Properties = BuildDefaultOperationSchemaProperties()
        });
    }


    private static Dictionary<string, OpenApiSchema> BuildDefaultOperationSchemaProperties() => new()
    {
        {
            "op", new OpenApiSchema
            {
                Type = SwaggerSchemaTypes.String,
                Enum = new List<IOpenApiAny>
                {
                    new OpenApiString("add"),
                    new OpenApiString("copy"),
                    new OpenApiString("move"),
                    new OpenApiString("remove"),
                    new OpenApiString("replace"),
                    new OpenApiString("test"),
                    new OpenApiString("invalid"),
                }
            }
        },
        {
            "path", new OpenApiSchema
            {
                Type = SwaggerSchemaTypes.String,
                Example = new OpenApiString("/path/to/property")
            }
        },
        {
            "from", new OpenApiSchema
            {
                Type = SwaggerSchemaTypes.String,
                Example = new OpenApiString("/path/to/property")
            }
        },
        {
            "value", new OpenApiSchema
            {
                Type = SwaggerSchemaTypes.String,
                Example = new OpenApiString("new value")
            }
        },
    };

    private static void FixJsonPatchDocumentSchemas(OpenApiDocument swaggerDoc)
    {
        var jsonPatchDocSchemas = swaggerDoc.Components.Schemas
            .Where(x => x.Key.EndsWith(nameof(JsonPatchDocument))
                || x.Value?.Properties != null
                && x.Value.Properties.Any(p => p.Value?.Reference?.Id == nameof(IContractResolver)))
            .Select(x => x.Value);

        foreach (var schema in jsonPatchDocSchemas)
        {
            schema.Properties = new Dictionary<string, OpenApiSchema>
            {
                ["operations"] = new()
                {
                    Type = SwaggerSchemaTypes.Array,
                    Description = "Array of operations to perform",
                    Items = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "PatchOperation",
                            Type = ReferenceType.Schema,
                        }
                    }
                }
            };
        }
    }
}