using Mapster;
using MediatR;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.Data.EntityFramework.Extensions;
using SeniorTemplate.Application.Features.Users.Models;
using SeniorTemplate.Data.Entities;

namespace SeniorTemplate.Application.Features.Users.Handlers;

internal class GetUserByUsernameHandler : IRequestHandler<GetUserByUsernameQuery, User>
{
	private readonly IDatabaseContext _database;
	public GetUserByUsernameHandler(IDatabaseContext database) => _database = database;


	public async Task<User> Handle(GetUserByUsernameQuery query, CancellationToken cancel)
	{
		var user = await _database.Set<AppUser>().Where(u => u.UserName == query.Username).FirstOr404Async(cancel);
		return user.Adapt<User>();
	}
}