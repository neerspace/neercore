using Microsoft.EntityFrameworkCore;
using SeniorTemplate.Data.Entities;

namespace SeniorTemplate.Data.Extensions;

public static class AppUserExtensions
{
	public static async Task<AppUser?> GetByLoginAsync(this IQueryable<AppUser> queryable, string login, CancellationToken cancel = default)
	{
		login = login.ToUpperInvariant();
		return await queryable
				.Where(e => e.NormalizedUserName == login || e.NormalizedEmail == login)
				.Include("Claims").Include("Roles.Role.Claims")
				.FirstOrDefaultAsync(cancel);
	}
}