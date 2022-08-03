using Mapster;
using NeerCore.Data;

namespace NeerCore.Mapping.Extensions;

public static class TypeAdapterConfigExtensions
{
    // TODO: To IRegister
    public static void AddDefaultConfigs(this TypeAdapterConfig config)
    {
        config.NewConfig<LocalizedString, string>()
            .MapWith(localized => localized.GetCurrentLocalization());

        config.NewConfig<string, LocalizedString>()
            .MapWith(rawString => new LocalizedString(rawString));
    }
}