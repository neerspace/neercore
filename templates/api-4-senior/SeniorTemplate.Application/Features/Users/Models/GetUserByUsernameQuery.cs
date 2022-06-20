using FluentValidation;
using MediatR;
using SeniorTemplate.Application.Extensions;

namespace SeniorTemplate.Application.Features.Users.Models;

public class GetUserByUsernameQuery : IRequest<User>
{
	/// <example>aspadmin</example>
	public string Username { get; init; }


	public GetUserByUsernameQuery(string username)
	{
		Username = username;
	}
}

public class GetUserByUsernameQueryValidator : AbstractValidator<GetUserByUsernameQuery>
{
	public GetUserByUsernameQueryValidator()
	{
		RuleFor(o => o.Username).NotEmpty().UserName();
	}
}