using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.DependencyInjection;
using NeerCore.Exceptions;
using NeerCoreTestingSuite.WebApp.Data.Entities;

namespace NeerCoreTestingSuite.WebApp.Services;

[Service]
public class TeasService
{
    private readonly IDatabase _database;
    // private readonly ISieveProcessor _sieveProcessor;

    public TeasService(IDatabase database)
    {
        _database = database;
    }

    public async Task<Tea> GetByIdAsync(Guid id)
    {
        var entity = await _database.Set<Tea>().FindAsync(id);
        return entity ?? throw new NotFoundException<Tea>();
    }

    public async Task<IEnumerable<Tea>> GetAllAsync()
    {
        return await _database.Set<Tea>().ToArrayAsync();
    }

    // public async Task<IEnumerable<Tea>> FilterAsync(string filters, string sorts, int page, int pageSize)
    // {
    //     var sieve = new SieveModel { Filters = filters, Sorts = sorts, Page = page, PageSize = pageSize };
    //     var queryable = _sieveProcessor.Apply(sieve, _database.Set<Tea>().AsNoTracking());
    //     return await queryable.ToListAsync();
    // }

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
        if (!await _database.Set<Tea>().AnyAsync(t => t.Id == tea.Id))
            throw new NotFoundException("Tea with given guid not found");
        _database.Set<Tea>().Update(tea);
        await _database.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        _database.Set<Tea>().Remove(entity);
        await _database.SaveChangesAsync();
    }
}