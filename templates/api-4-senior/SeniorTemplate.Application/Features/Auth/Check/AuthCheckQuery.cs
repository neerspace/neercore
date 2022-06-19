using FluentValidation;
using MediatR;
using SeniorTemplate.Application.Extensions;

namespace SeniorTemplate.Application.Features.Auth.Check;

public class AuthCheckQuery : IRequest<AuthCheckResult>
{
	/// <example>aspadmin</example>
	public string Login { get; init; } = default!;
}

public class AuthCheckQueryValidator : AbstractValidator<AuthCheckQuery>
{
	public AuthCheckQueryValidator()
	{
		RuleFor(o => o.Login).NotEmpty().UserName();
	}
}