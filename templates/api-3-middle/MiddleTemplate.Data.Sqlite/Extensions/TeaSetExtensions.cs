using Microsoft.EntityFrameworkCore;
using MiddleTemplate.Data.Entities;
using NeerCore.Exceptions;

namespace MiddleTemplate.Data.Extensions;

public static class TeaSetExtensions
{
	public static async Task<Tea> GetByIdAsync(this DbSet<Tea> teasSet, Guid id, CancellationToken cancel = default)
	{
		var entity = await teasSet.FindAsync(new object[] { id }, cancel);
		return entity ?? throw new NotFoundException<Tea>();
	}
}