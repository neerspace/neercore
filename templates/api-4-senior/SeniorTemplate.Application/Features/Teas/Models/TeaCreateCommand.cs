using FluentValidation;
using MediatR;

namespace SeniorTemplate.Application.Features.Teas.Models;

public class TeaCreateCommand : IRequest<TeaModel>
{
	/// <example>Black tea</example>
	public string Name { get; init; } = default!;

	/// <example>19.50</example>
	public decimal Price { get; init; }
}

internal class CreateTeaCommandValidator : AbstractValidator<TeaCreateCommand>
{
	public CreateTeaCommandValidator()
	{
		RuleFor(o => o.Name).NotEmpty().Length(2, 64);
		RuleFor(o => o.Price).GreaterThan(0m);
	}
}