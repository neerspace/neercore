using Microsoft.AspNetCore.Identity;
using NeerCore.Data.Abstractions;

namespace SeniorTemplate.Data.Entities;

public class AppUser : IdentityUser<int>, IEntity
{
	public string? Description { get; set; }
	public DateTime Registered { get; set; } = DateTime.UtcNow;


	public virtual ICollection<AppUserRole>? Roles { get; set; }
	public virtual ICollection<AppUserClaim>? Claims { get; set; }
	public virtual ICollection<AppUserLogin>? Logins { get; set; }
	public virtual ICollection<AppRefreshToken>? RefreshTokens { get; set; }
	public virtual ICollection<AppUserToken>? Tokens { get; set; }
}