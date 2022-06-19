using Microsoft.AspNetCore.Identity;
using NeerCore.Data.Abstractions;

namespace SeniorTemplate.Data.Entities;

public class AppRole : IdentityRole<int>, IEntity
{
	public virtual ICollection<AppUserRole>? Users { get; set; }
	public virtual ICollection<AppRoleClaim>? Claims { get; set; }
}