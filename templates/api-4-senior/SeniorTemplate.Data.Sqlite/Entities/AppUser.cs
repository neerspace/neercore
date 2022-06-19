using Microsoft.AspNetCore.Identity;
using NeerCore.Data.Abstractions;

namespace SeniorTemplate.Data.Entities;

public class AppUser : IdentityUser<long>, IEntity
{
	public override long Id { get; set; }
	public override string UserName { get; set; } = default!;
	public override string NormalizedUserName { get; set; } = default!;
	public override string Email { get; set; } = default!;
	public override string NormalizedEmail { get; set; } = default!;
	public override bool EmailConfirmed { get; set; }
	public string? Description { get; set; }
	public override string? PasswordHash { get; set; }
	public override string SecurityStamp { get; set; } = default!;
	public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
	public override string? PhoneNumber { get; set; }
	public override bool PhoneNumberConfirmed { get; set; }
	public override bool TwoFactorEnabled { get; set; }
	public override DateTimeOffset? LockoutEnd { get; set; }
	public override bool LockoutEnabled { get; set; }
	public override int AccessFailedCount { get; set; }
	public DateTime Registered { get; set; } = DateTime.UtcNow;


	public virtual ICollection<AppUserRole>? Roles { get; set; }
	public virtual ICollection<AppUserClaim>? Claims { get; set; }
	public virtual ICollection<AppUserLogin>? Logins { get; set; }
	public virtual ICollection<AppRefreshToken>? RefreshTokens { get; set; }
	public virtual ICollection<AppUserToken>? Tokens { get; set; }
}