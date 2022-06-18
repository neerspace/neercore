using MediatR;
using MiddleTemplate.Data.Entities;
using MiddleTemplate.Data.Extensions;
using NeerCore.Data.EntityFramework.Abstractions;
using SeniorTemplate.Application.Features.Teas.Models;

namespace SeniorTemplate.Application.Features.Teas.Handlers;

public class TeaDeleteHandler : IRequestHandler<TeaDeleteCommand>
{
	private readonly IDatabaseContext _database;
	public TeaDeleteHandler(IDatabaseContext database) => _database = database;


	public async Task<Unit> Handle(TeaDeleteCommand request, CancellationToken cancellationToken)
	{
		var entity = await _database.Set<Tea>().GetByIdAsync(request.Id, cancellationToken);
		_database.Set<Tea>().Remove(entity);
		await _database.SaveChangesAsync(cancellationToken);
		return Unit.Value;
	}
}