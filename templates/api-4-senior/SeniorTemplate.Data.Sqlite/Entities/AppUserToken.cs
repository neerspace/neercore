using Microsoft.AspNetCore.Identity;
using NeerCore.Data.Abstractions;

namespace SeniorTemplate.Data.Entities;

public class AppUserToken : IdentityUserToken<long>, IEntity
{
	public override long UserId { get; set; }
	public override string? LoginProvider { get; set; }
	public override string Name { get; set; } = default!;
	public override string Value { get; set; } = default!;
	public DateTime Created { get; set; } = DateTime.UtcNow;


	public virtual AppUser? User { get; set; }
}