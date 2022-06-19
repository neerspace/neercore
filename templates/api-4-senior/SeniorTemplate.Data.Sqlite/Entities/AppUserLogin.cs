using Microsoft.AspNetCore.Identity;
using NeerCore.Data.Abstractions;

namespace SeniorTemplate.Data.Entities;

public class AppUserLogin : IdentityUserLogin<int>, IEntity
{
	public virtual AppUser? User { get; set; }
}