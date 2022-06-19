using System.Text.Json.Serialization;
using FluentValidation;
using MediatR;

namespace SeniorTemplate.Application.Features.Teas.Models;

public class TeaUpdateCommand : IRequest<TeaModel>
{
	[JsonIgnore] public Guid Id { get; init; }

	/// <example>Black tea</example>
	public string Name { get; init; } = default!;

	/// <example>19.50$</example>
	public string Price { get; init; } = default!;
}

internal class TeaUpdateCommandValidator : AbstractValidator<TeaUpdateCommand>
{
	public TeaUpdateCommandValidator()
	{
		RuleFor(o => o.Name).NotEmpty().MaximumLength(128);
		RuleFor(o => o.Price).NotEmpty();
	}
}