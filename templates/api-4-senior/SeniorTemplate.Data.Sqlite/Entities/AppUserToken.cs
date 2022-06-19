using Microsoft.AspNetCore.Identity;
using NeerCore.Data.Abstractions;

namespace SeniorTemplate.Data.Entities;

public class AppUserToken : IdentityUserToken<int>, IEntity
{
	public DateTime Created { get; set; } = DateTime.UtcNow;


	public virtual AppUser? User { get; set; }
}