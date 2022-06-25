using System.Text.Json;
using NeerCore.Extensions;

namespace NeerCore.Json.Policies;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
	public static SnakeCaseNamingPolicy Instance { get; } = new();

	public override string ConvertName(string name)
	{
		return name.ToSnakeCase();
	}
}