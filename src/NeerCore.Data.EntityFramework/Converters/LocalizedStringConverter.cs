using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NeerCore.Data.EntityFramework.Converters;

public class LocalizedStringConverter : ValueConverter<LocalizedString, string>
{
    public LocalizedStringConverter()
        : base(
            v => v.ToString(),
            v => new LocalizedString(v)
    )
    { }
}