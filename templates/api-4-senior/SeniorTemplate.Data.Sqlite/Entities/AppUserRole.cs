using Microsoft.AspNetCore.Identity;
using NeerCore.Data.Abstractions;

namespace SeniorTemplate.Data.Entities;

public class AppUserRole : IdentityUserRole<int>, IEntity
{
	public virtual AppUser? User { get; set; }
	public virtual AppRole? Role { get; set; }
}