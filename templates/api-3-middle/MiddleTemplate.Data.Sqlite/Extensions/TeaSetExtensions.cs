using Microsoft.EntityFrameworkCore;
using MiddleTemplate.Data.Entities;
using NeerCore.Exceptions;

namespace MiddleTemplate.Data.Extensions;

public static class TeaSetExtensions
{
	public static async Task<Tea> GetByIdAsync(this DbSet<Tea> teasSet, Guid id, CancellationToken cancellationToken = default)
	{
		var entity = await teasSet.FindAsync(new object[] { id }, cancellationToken);
		return entity ?? throw new NotFoundException<Tea>();
	}
}