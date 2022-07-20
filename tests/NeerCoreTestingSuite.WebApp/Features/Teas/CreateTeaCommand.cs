using FluentValidation;
using NeerCore.Application.Abstractions;
using NeerCoreTestingSuite.WebApp.Dto.Teas;

namespace NeerCoreTestingSuite.WebApp.Features.Teas;

public class CreateTeaCommand : ICommand<Tea>
{
	/// <example>Black tea</example>
	public string Name { get; init; } = default!;

	/// <example>19.50</example>
	public decimal Price { get; init; }
}

public class TeaCreateValidator : AbstractValidator<CreateTeaCommand>
{
	public TeaCreateValidator()
	{
		RuleFor(o => o.Name).NotEmpty().Length(2, 64);
		RuleFor(o => o.Price).GreaterThan(0m);
	}
}