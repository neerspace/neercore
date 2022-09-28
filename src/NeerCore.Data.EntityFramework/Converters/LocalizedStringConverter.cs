using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NeerCore.Data.EntityFramework.Converters;

public sealed class LocalizedStringConverter : ValueConverter<LocalizedString, string>
{
    public LocalizedStringConverter()
            : base(
                v => v.ToString(),
                v => new LocalizedString(v)
            ) { }
}