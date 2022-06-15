using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeerCore.Api.DocumentFilters;

public class JsonPatchDocumentFilter : IDocumentFilter
{
	public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
	{
		var schemas = swaggerDoc.Components.Schemas
				.Where(item => item.Key.StartsWith("Operation") || item.Key.StartsWith("JsonPatchDocument"));
		foreach (var item in schemas)
			swaggerDoc.Components.Schemas.Remove(item.Key);

		swaggerDoc.Components.Schemas.Add("Operation", new OpenApiSchema
		{
			Type = "object",
			Properties = new Dictionary<string, OpenApiSchema>
			{
				{ "op", new OpenApiSchema { Type = "string" } },
				{ "value", new OpenApiSchema { Type = "string" } },
				{ "path", new OpenApiSchema { Type = "string" } }
			}
		});

		swaggerDoc.Components.Schemas.Add("JsonPatchDocument", new OpenApiSchema
		{
			Type = "array",
			Items = new OpenApiSchema
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.Schema,
					Id = "Operation"
				}
			},
			Description = "Array of operations to perform"
		});

		foreach (var path in swaggerDoc.Paths.SelectMany(p => p.Value.Operations).Where(p => p.Key == OperationType.Patch))
		{
			foreach (var item in path.Value.RequestBody.Content.Where(c => !c.Key.StartsWith("application/json")))
				path.Value.RequestBody.Content.Remove(item.Key);
			var response = path.Value.RequestBody.Content.Single(c => c.Key.StartsWith("application/json"));
			response.Value.Schema = new OpenApiSchema
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.Schema,
					Id = "JsonPatchDocument"
				}
			};
		}
	}
}