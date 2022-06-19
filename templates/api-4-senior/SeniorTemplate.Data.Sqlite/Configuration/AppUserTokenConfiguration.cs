using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeniorTemplate.Data.Entities;

namespace SeniorTemplate.Data.Configuration;

internal class AppUserTokenConfiguration : IEntityTypeConfiguration<AppUserToken>
{
	public void Configure(EntityTypeBuilder<AppUserToken> builder)
	{
		builder.Property(e => e.LoginProvider).HasMaxLength(64);
		builder.Property(e => e.Name).HasMaxLength(128);
		builder.Property(e => e.Value).HasMaxLength(256);

		builder.ToTable("AppUserTokens");
	}
}