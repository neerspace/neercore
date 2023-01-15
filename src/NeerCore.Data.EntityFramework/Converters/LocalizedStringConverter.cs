using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NeerCore.Localization;

namespace NeerCore.Data.EntityFramework.Converters;

public sealed class LocalizedStringConverter : ValueConverter<LocalizedString, string>
{
    public LocalizedStringConverter()
            : base(
                v => v.ToString(),
                v => new LocalizedString(v)
            ) { }
}