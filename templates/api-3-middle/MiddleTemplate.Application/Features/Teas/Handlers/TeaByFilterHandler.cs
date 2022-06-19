using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiddleTemplate.Application.Features.Teas.Models;
using MiddleTemplate.Data.Entities;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.Mapping.Extensions;
using Sieve.Models;
using Sieve.Services;

namespace MiddleTemplate.Application.Features.Teas.Handlers;

public class TeaByFilterHandler : IRequestHandler<TeaByFilterQuery, IEnumerable<TeaModel>>
{
	private readonly IDatabaseContext _database;
	private readonly ISieveProcessor _sieveProcessor;

	public TeaByFilterHandler(IDatabaseContext database, ISieveProcessor sieveProcessor)
	{
		_database = database;
		_sieveProcessor = sieveProcessor;
	}


	public async Task<IEnumerable<TeaModel>> Handle(TeaByFilterQuery request, CancellationToken cancel)
	{
		var sieve = request.Adapt<SieveModel>();
		var queryable = _sieveProcessor.Apply(sieve, _database.Set<Tea>().AsNoTracking());
		var entities = await queryable.ToListAsync(cancel);
		return entities.AdaptAll<TeaModel>();
	}
}