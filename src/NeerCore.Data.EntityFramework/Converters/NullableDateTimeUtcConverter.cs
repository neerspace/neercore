using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NeerCore.Data.EntityFramework.Converters;

public sealed class NullableDateTimeUtcConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableDateTimeUtcConverter()
        : base(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v
        ) { }
}