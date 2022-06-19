using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeniorTemplate.Data.Entities;

namespace SeniorTemplate.Data.Configuration;

internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
	public void Configure(EntityTypeBuilder<AppUser> builder)
	{
		builder.Property(e => e.Description).HasMaxLength(256);
		builder.Property(e => e.SecurityStamp).HasMaxLength(64);
		builder.Property(e => e.ConcurrencyStamp).HasMaxLength(64);
		builder.Property(e => e.PhoneNumber).HasMaxLength(32);

		builder.HasMany(e => e.Roles).WithOne(e => e.User)
				.HasForeignKey(e => e.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
		builder.HasMany(e => e.Claims).WithOne(e => e.User)
				.HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
		builder.HasMany(e => e.Logins).WithOne(e => e.User)
				.HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
		builder.HasMany(e => e.RefreshTokens).WithOne(e => e.User)
				.HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
		builder.HasMany(e => e.Tokens).WithOne(e => e.User)
				.HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

		builder.ToTable("AppUsers");
	}
}