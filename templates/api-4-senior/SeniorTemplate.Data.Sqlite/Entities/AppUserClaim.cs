using Microsoft.AspNetCore.Identity;
using NeerCore.Data.Abstractions;

namespace SeniorTemplate.Data.Entities;

public class AppUserClaim : IdentityUserClaim<int>, IEntity
{
	public virtual AppUser? User { get; set; }
}