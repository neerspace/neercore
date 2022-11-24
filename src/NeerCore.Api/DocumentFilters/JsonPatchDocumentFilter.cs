using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using OperationType = Microsoft.AspNetCore.JsonPatch.Operations.OperationType;

namespace NeerCore.Api.DocumentFilters;

/// <summary>
///   Swagger document filter to hide redundant models from
///   <see cref="Microsoft.AspNetCore.JsonPatch.JsonPatchDocument{TModel}"/>.
/// </summary>
public sealed class JsonPatchDocumentFilter : IDocumentFilter
{
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
        IEnumerable<IOpenApiAny> OperationPathFilter(string pathName, OpenApiSchema pathSchema, int arrayDepth = 0)
        {
            yield return new OpenApiString(pathName);

            // pathSchema.Type == SchemaTypes.Object
            if (pathSchema.Reference is not null)
            {
                var properties = pathSchema.Properties.Count == 0
                    ? swaggerDoc.Components.Schemas[pathSchema.Reference.Id].Properties
                    : pathSchema.Properties;

                foreach (var pathSchemaProperty in properties)
                {
                    string nextPathName = $"{pathName}/{pathSchemaProperty.Key}";
                    foreach (var operationItem in OperationPathFilter(nextPathName, pathSchemaProperty.Value))
                        yield return operationItem;
                }
            }
            // pathSchema.Type == SchemaTypes.Array
            else if (pathSchema.Items is not null)
            {
                string nextPathName = $"{pathName}/{{{arrayDepth}}}";
                foreach (var operationItem in OperationPathFilter(nextPathName, pathSchema.Items, arrayDepth + 1))
                    yield return operationItem;
            }
        }

        swaggerDoc.Components.Schemas.Remove(nameof(OperationType));
        var operationSchemas = swaggerDoc.Components.Schemas.Where(item => item.Key.EndsWith(nameof(Operation)));
        foreach (var operationSchema in operationSchemas)
        {
            string baseName = operationSchema.Key.Replace(nameof(Operation), "");
            if (swaggerDoc.Components.Schemas.ContainsKey(baseName))
            {
                var baseSchema = swaggerDoc.Components.Schemas[baseName];
                var basePropertyNames = baseSchema.Properties.SelectMany(p => OperationPathFilter("/" + p.Key, p.Value)).ToArray();

                operationSchema.Value.Properties = new Dictionary<string, OpenApiSchema>
                {
                    {
                        "op", new OpenApiSchema
                        {
                            Type = SchemaTypes.String,
                            Enum = OperationNameEnum
                        }
                    },
                    {
                        "path", new OpenApiSchema
                        {
                            Type = SchemaTypes.String,
                            Enum = basePropertyNames
                        }
                    },
                    {
                        "from", new OpenApiSchema
                        {
                            Type = SchemaTypes.String,
                            Enum = basePropertyNames
                        }
                    },
                    {
                        "value", new OpenApiSchema
                        {
                            Type = SchemaTypes.String,
                            Example = new OpenApiString("new value")
                        }
                    },
                };
            }
            else
            {
                operationSchema.Value.Properties = new Dictionary<string, OpenApiSchema>
                {
                    {
                        "op", new OpenApiSchema
                        {
                            Type = SchemaTypes.String,
                            Enum = OperationNameEnum
                        }
                    },
                    {
                        "path", new OpenApiSchema
                        {
                            Type = SchemaTypes.String,
                            Example = new OpenApiString("/path/to/property")
                        }
                    },
                    {
                        "from", new OpenApiSchema
                        {
                            Type = SchemaTypes.String,
                            Example = new OpenApiString("/path/to/property")
                        }
                    },
                    {
                        "value", new OpenApiSchema
                        {
                            Type = SchemaTypes.String,
                            Example = new OpenApiString("new value")
                        }
                    },
                };
            }
        }
    }

    private static void FixJsonPatchDocumentSchemas(OpenApiDocument swaggerDoc)
    {
        var jsonPatchDocSchemas = swaggerDoc.Components.Schemas.Where(item => item.Key.EndsWith(nameof(JsonPatchDocument)));
        foreach (var jsonPatchDocSchema in jsonPatchDocSchemas)
        {
            string baseName = jsonPatchDocSchema.Key.Replace(nameof(JsonPatchDocument), "");
            var schema = jsonPatchDocSchema.Value;
            schema.Properties = new Dictionary<string, OpenApiSchema>
            {
                ["operations"] = new()
                {
                    Type = SchemaTypes.Array,
                    Description = "Array of operations to perform.",
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
        }
    }

    private static List<IOpenApiAny> OperationNameEnum => new()
    {
        new OpenApiString("add"),
        new OpenApiString("copy"),
        new OpenApiString("move"),
        new OpenApiString("remove"),
        new OpenApiString("replace"),
        new OpenApiString("test"),
        new OpenApiString("invalid"),
    };

    private static class SchemaTypes
    {
        public const string Object = "object";
        public const string Array = "array";
        public const string String = "string";
    }
}