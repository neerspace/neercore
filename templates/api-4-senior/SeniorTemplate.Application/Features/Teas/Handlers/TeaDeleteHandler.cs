using MediatR;
using NeerCore.Data.EntityFramework.Abstractions;
using SeniorTemplate.Application.Features.Teas.Models;
using SeniorTemplate.Data.Entities;
using SeniorTemplate.Data.Extensions;

namespace SeniorTemplate.Application.Features.Teas.Handlers;

public class TeaDeleteHandler : IRequestHandler<TeaDeleteCommand>
{
	private readonly IDatabaseContext _database;
	public TeaDeleteHandler(IDatabaseContext database) => _database = database;


	public async Task<Unit> Handle(TeaDeleteCommand request, CancellationToken cancel)
	{
		var entity = await _database.Set<Tea>().GetByIdAsync(request.Id, cancel);
		_database.Set<Tea>().Remove(entity);
		await _database.SaveChangesAsync(cancel);
		return Unit.Value;
	}
}