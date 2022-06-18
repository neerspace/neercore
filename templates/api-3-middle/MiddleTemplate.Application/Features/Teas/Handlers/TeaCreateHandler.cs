using Mapster;
using MediatR;
using MiddleTemplate.Application.Features.Teas.Models;
using MiddleTemplate.Data.Entities;
using NeerCore.Data.EntityFramework.Abstractions;

namespace MiddleTemplate.Application.Features.Teas.Handlers;

public class TeaCreateHandler : IRequestHandler<TeaCreateCommand, TeaModel>
{
	private readonly IDatabaseContext _database;
	public TeaCreateHandler(IDatabaseContext database) => _database = database;


	public async Task<TeaModel> Handle(TeaCreateCommand request, CancellationToken cancellationToken)
	{
		var entity = request.Adapt<Tea>();
		_database.Set<Tea>().Add(entity);
		await _database.SaveChangesAsync(cancellationToken);
		return entity.Adapt<TeaModel>();
	}
}