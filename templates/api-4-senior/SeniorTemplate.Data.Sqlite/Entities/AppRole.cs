using Microsoft.AspNetCore.Identity;
using NeerCore.Data.Abstractions;

namespace SeniorTemplate.Data.Entities;

public class AppRole : IdentityRole<long>, IEntity
{
	public override long Id { get; set; }
	public override string Name { get; set; } = default!;
	public override string NormalizedName { get; set; } = default!;
	public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();


	public virtual ICollection<AppUserRole>? Users { get; set; }
	public virtual ICollection<AppRoleClaim>? Claims { get; set; }
}