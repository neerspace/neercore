using FluentValidation;
using MediatR;
using SeniorTemplate.Application.Extensions;

namespace SeniorTemplate.Application.Features.Auth.Complete;

public class AuthCompleteCommand : IRequest<AuthResult>
{
	/// <example>aspadmin</example>
	public string Login { get; init; } = default!;

	/// <example>aspX1234</example>
	public string? Password { get; init; }
}

public class AuthCompleteCommandValidator : AbstractValidator<AuthCompleteCommand>
{
	public AuthCompleteCommandValidator()
	{
		RuleFor(o => o.Login).NotEmpty().UserName();
		RuleFor(o => o.Password).NotEmpty();
	}
}