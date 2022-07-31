using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NeerCore.Data.EntityFramework.Converters;

public class DateTimeUtcConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeUtcConverter()
        : base(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
        ) { }
}