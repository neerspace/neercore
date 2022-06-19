using JuniorTemplate.Application.Services;
using JuniorTemplate.Data.Entities;
using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.DependencyInjection;
using NeerCore.Exceptions;
using Sieve.Models;
using Sieve.Services;

namespace JuniorTemplate.Infrastructure.Services;

[Inject]
public class TeasService : ITeasService
{
	private readonly IDatabaseContext _database;
	private readonly ISieveProcessor _sieveProcessor;

	public TeasService(IDatabaseContext database, ISieveProcessor sieveProcessor)
	{
		_database = database;
		_sieveProcessor = sieveProcessor;
	}

	public async Task<Tea> GetByIdAsync(Guid id)
	{
		var entity = await _database.Set<Tea>().FindAsync(id);
		return entity ?? throw new NotFoundException<Tea>();
	}

	public async Task<IEnumerable<Tea>> FilterAsync(string filters, string sorts, int page, int pageSize)
	{
		var sieve = new SieveModel { Filters = filters, Sorts = sorts, Page = page, PageSize = pageSize };
		var queryable = _sieveProcessor.Apply(sieve, _database.Set<Tea>().AsNoTracking());
		return await queryable.ToListAsync();
	}

	public async Task<int> CountAsync()
	{
		return await _database.Set<Tea>().CountAsync();
	}

	public async Task CreateAsync(Tea tea)
	{
		_database.Set<Tea>().Add(tea);
		await _database.SaveChangesAsync();
	}

	public async Task UpdateAsync(Tea tea)
	{
		_database.Set<Tea>().Update(tea);
		await _database.SaveChangesAsync();
	}

	public async Task DeleteAsync(Guid id)
	{
		var entity = await GetByIdAsync(id);
		_database.Set<Tea>().Add(entity);
		await _database.SaveChangesAsync();
	}
}