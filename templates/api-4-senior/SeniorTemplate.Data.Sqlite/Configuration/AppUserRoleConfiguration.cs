using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeniorTemplate.Data.Entities;

namespace SeniorTemplate.Data.Configuration;

internal class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
{
	public void Configure(EntityTypeBuilder<AppUserRole> builder)
	{
		builder.HasKey(e => new { e.UserId, e.RoleId });

		builder.Property(e => e.RoleId).HasColumnType("smallint");

		builder.HasOne(e => e.Role).WithMany(e => e.Users)
				.HasForeignKey(e => e.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);

		builder.ToTable("AppUserRoles");
	}
}