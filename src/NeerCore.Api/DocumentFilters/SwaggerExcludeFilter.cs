using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NeerCore.Api.DocumentFilters;

public class SwaggerExcludeFilter : ISchemaFilter
{
	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		if (schema.Properties == null)
			return;

		var excludedProperties = type.GetProperties()
				.Where(t =>
						t.GetCustomAttribute<SwaggerExcludeAttribute>()
						!= null);

		foreach (var excludedProperty in excludedProperties)
		{
			if (schema.Properties.ContainsKey(excludedProperty.Name))
				schema.Properties.Remove(excludedProperty.Name);
		}
	}
}