using Microsoft.AspNetCore.Identity;
using NeerCore.Data.Abstractions;

namespace SeniorTemplate.Data.Entities;

public class AppRoleClaim : IdentityRoleClaim<int>, IEntity
{
	public virtual AppRole? Role { get; set; }
}