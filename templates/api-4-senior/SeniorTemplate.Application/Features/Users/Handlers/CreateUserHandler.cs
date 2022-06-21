using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NeerCore.Exceptions;
using SeniorTemplate.Application.Extensions;
using SeniorTemplate.Application.Features.Users.Models;
using SeniorTemplate.Core.Constants;
using SeniorTemplate.Data.Entities;

namespace SeniorTemplate.Application.Features.Users.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
{
	private readonly UserManager<AppUser> _userManager;
	public CreateUserHandler(UserManager<AppUser> userManager) => _userManager = userManager;


	public async Task<User> Handle(CreateUserCommand command, CancellationToken cancel)
	{
		var user = new AppUser { UserName = command.Username, Email = command.Email };

		IdentityResult? result = await _userManager.CreateAsync(user);
		if (!result.Succeeded)
			throw new ValidationFailedException("User not created.", result.ToErrorDetails());

		await _userManager.AddPasswordAsync(user, command.Password);
		await _userManager.AddToRoleAsync(user, Roles.User);

		return user.Adapt<User>();
	}
}