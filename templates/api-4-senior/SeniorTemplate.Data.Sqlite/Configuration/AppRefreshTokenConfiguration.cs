using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeniorTemplate.Data.Entities;

namespace SeniorTemplate.Data.Configuration;

internal class AppRefreshTokenConfiguration : IEntityTypeConfiguration<AppRefreshToken>
{
	public void Configure(EntityTypeBuilder<AppRefreshToken> builder)
	{
		builder.HasKey(e => e.Token);
		builder.Property(e => e.Token).HasColumnType("varchar(128)");
		builder.Property(e => e.UserAgent).HasMaxLength(512).IsUnicode(false);
		builder.Property(e => e.IpAddress).HasMaxLength(46).IsUnicode(false);

		builder.ToTable("AppRefreshTokens");
	}
}