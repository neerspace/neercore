using System.Text.Json;
using System.Text.Json.Serialization;

namespace NeerCore.Globals;

public static class GlobalJsonConventions
{
	public static JsonSerializerOptions CamelCase { get; set; } = new()
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	};

	public static JsonSerializerOptions ExtendedScheme { get; set; } = new()
	{
		ReadCommentHandling = JsonCommentHandling.Skip
	};
}