using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiddleTemplate.Data.Entities;

namespace MiddleTemplate.Data.Configuration;

internal class TeaConfiguration : IEntityTypeConfiguration<Tea>
{
	public void Configure(EntityTypeBuilder<Tea> builder)
	{
		builder.HasKey(e => e.Id);
		builder.Property(e => e.Name).HasMaxLength(128);
		builder.Property(e => e.Updated).ValueGeneratedOnUpdate();
		builder.Property(e => e.Created).ValueGeneratedOnAdd();

		builder.ToTable("Teas");
	}
}