using Mapster;
using MediatR;
using NeerCore.Data.EntityFramework.Abstractions;
using SeniorTemplate.Application.Features.Teas.Models;
using SeniorTemplate.Data.Entities;
using SeniorTemplate.Data.Extensions;

namespace SeniorTemplate.Application.Features.Teas.Handlers;

public class TeaByIdHandler : IRequestHandler<TeaByIdQuery, TeaModel>
{
	private readonly IDatabaseContext _database;
	public TeaByIdHandler(IDatabaseContext database) => _database = database;


	public async Task<TeaModel> Handle(TeaByIdQuery request, CancellationToken cancel)
	{
		var entity = await _database.Set<Tea>().GetByIdAsync(request.Id, cancel);
		return entity.Adapt<TeaModel>();
	}
}