using JuniorTemplate.Data.Entities;

namespace JuniorTemplate.Application.Services;

public interface ITeasService
{
	public Task<Tea> GetByIdAsync(Guid id);
	public Task<IEnumerable<Tea>> FilterAsync(string filters, string sorts, int page, int pageSize);
	public Task<int> CountAsync();
	public Task CreateAsync(Tea tea);
	public Task UpdateAsync(Tea tea);
	public Task DeleteAsync(Guid id);
}