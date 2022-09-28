using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework.Comparers;
using NeerCore.Data.EntityFramework.Converters;

namespace NeerCore.Data.EntityFramework.Extensions;

public static class ModelConfigurationBuilderExtensions
{
    public static void ConfigureDataConversions(this ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.ApplyLocalizedStringConversions();
        configurationBuilder.ApplyUtcDateTimeConversions();
    }

    public static void ApplyUtcDateTimeConversions(this ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeUtcConverter>();
        configurationBuilder.Properties<DateTime?>().HaveConversion<NullableDateTimeUtcConverter>();
    }

    public static void ApplyLocalizedStringConversions(this ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<LocalizedString>().HaveConversion<LocalizedStringConverter, LocalizedStringComparer>();
    }
}