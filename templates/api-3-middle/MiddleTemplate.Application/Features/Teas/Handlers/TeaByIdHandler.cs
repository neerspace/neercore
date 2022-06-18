using Mapster;
using MediatR;
using MiddleTemplate.Application.Features.Teas.Models;
using MiddleTemplate.Data.Entities;
using MiddleTemplate.Data.Extensions;
using NeerCore.Data.EntityFramework.Abstractions;

namespace MiddleTemplate.Application.Features.Teas.Handlers;

public class TeaByIdHandler : IRequestHandler<TeaByIdQuery, TeaModel>
{
	private readonly IDatabaseContext _database;
	public TeaByIdHandler(IDatabaseContext database) => _database = database;


	public async Task<TeaModel> Handle(TeaByIdQuery request, CancellationToken cancellationToken)
	{
		var entity = await _database.Set<Tea>().GetByIdAsync(request.Id, cancellationToken);
		return entity.Adapt<TeaModel>();
	}
}