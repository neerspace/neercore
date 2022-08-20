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
        var jsonPatchDocSchemas = swaggerDoc.Components.Schemas
            .Where(item => item.Key.Equals("Operation") || item.Key.Equals("IContractResolver"));
        foreach (var jsonPatchDocSchema in jsonPatchDocSchemas)
            swaggerDoc.Components.Schemas.Remove(jsonPatchDocSchema.Key);

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

        // Fix *JsonPatchDocument schemas
        jsonPatchDocSchemas = swaggerDoc.Components.Schemas
            .Where(item => item.Key.Contains("JsonPatchDocument"));
        foreach (var jsonPatchDocSchema in jsonPatchDocSchemas)
        {
            var schema = jsonPatchDocSchema.Value;
            schema.Type = "array";
            schema.Properties.Clear();
            schema.AdditionalPropertiesAllowed = true;
            schema.Items = new OpenApiSchema
            {
                Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "Operation" }
            };
            if (schema.Description.IsNullOrEmpty())
                schema.Description = "Array of operations to perform";
        }

        // foreach (var path in swaggerDoc.Paths
        //              .SelectMany(p => p.Value.Operations)
        //              .Where(p => p.Key == OperationType.Patch))
        // {
        //     foreach (var item in path.Value.RequestBody.Content
        //                  .Where(c => !c.Key.StartsWith("application/json") && !c.Key.StartsWith("application/patch+json")))
        //         path.Value.RequestBody.Content.Remove(item.Key);
        //
        //     var response = path.Value.RequestBody.Content.Single(c => c.Key.StartsWith("application/json"));
        //     response.Value.Schema = new OpenApiSchema
        //     {
        //         Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "JsonPatchDocument" }
        //     };
        // }
    }
}