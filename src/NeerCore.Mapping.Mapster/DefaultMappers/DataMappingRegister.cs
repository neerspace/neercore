using Mapster;
using NeerCore.Localization;

namespace NeerCore.Mapping.DefaultMappers;

public class DataMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LocalizedString, string>()
            .MapWith(localized => localized.GetCurrentLocalization());

        config.NewConfig<string, LocalizedString>()
            .MapWith(rawString => new LocalizedString(rawString));
    }
}