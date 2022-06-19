using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeniorTemplate.Core.Constants;
using SeniorTemplate.Data.Entities;

namespace SeniorTemplate.Data.SeedData;

public static partial class SeedExtensions
{
	public static void SeedDefaultUser(this ModelBuilder builder)
	{
		builder.Entity<AppUser>().HasData(Users);
		builder.Entity<AppUserClaim>().HasData(UserClaims);
	}

	private static readonly PasswordHasher<AppUser?> hasher = new();

	private static readonly AppUser[] Users =
	{
		new()
		{
			Id = 1,
			UserName = "aspadmin",
			NormalizedUserName = "ASPADMIN",
			Email = "aspadmin@asp.net",
			NormalizedEmail = "ASPADMIN@ASP.NET",
			EmailConfirmed = true,
			PasswordHash = hasher.HashPassword(null, "aspX1234"),
			SecurityStamp = Guid.NewGuid().ToString()
		},
	};


	private static readonly AppUserClaim[] UserClaims =
	{
		new()
		{
			Id = 1,
			UserId = 1,
			ClaimType = Claims.Permission,
			ClaimValue = Permissions.Master
		}
	};
}