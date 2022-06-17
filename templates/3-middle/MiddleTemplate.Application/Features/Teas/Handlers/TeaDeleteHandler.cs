using MediatR;
using MiddleTemplate.Application.Features.Teas.Models;
using MiddleTemplate.Data.Entities;
using MiddleTemplate.Data.Extensions;
using NeerCore.Data.EntityFramework.Abstractions;

namespace MiddleTemplate.Application.Features.Teas.Handlers;

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