using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeniorTemplate.Data.Entities;

namespace SeniorTemplate.Data.Configuration;

internal class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
	public void Configure(EntityTypeBuilder<AppRole> builder)
	{
		// builder.Property(e => e.Id).HasColumnType("smallint");
		builder.Property(e => e.Name).HasMaxLength(64);
		builder.Property(e => e.NormalizedName).HasMaxLength(64);
		builder.Property(e => e.ConcurrencyStamp).HasMaxLength(64);

		builder.HasMany(e => e.Claims).WithOne(e => e.Role)
				.HasForeignKey(e => e.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);

		builder.ToTable("AppRoles");
	}
}