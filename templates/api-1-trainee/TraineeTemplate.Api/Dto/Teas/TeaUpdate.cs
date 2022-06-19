using System.Text.Json.Serialization;

namespace TraineeTemplate.Api.Dto;

public record TeaUpdate
{
	[JsonIgnore] public Guid Id { get; init; }

	/// <example>Black tea</example>
	public string Name { get; init; } = default!;

	/// <example>19.50$</example>
	public string Price { get; init; } = default!;
}