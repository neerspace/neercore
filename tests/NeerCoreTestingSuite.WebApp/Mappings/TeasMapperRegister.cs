using Mapster;
using NeerCoreTestingSuite.WebApp.Data.Entities;

namespace NeerCoreTestingSuite.WebApp.Mappings;

public class TeasMapperRegister : IRegister
{
    private const decimal BtcRate = 0.000034m;

    public void Register(TypeAdapterConfig config)
    {
        // m -> member
        // s -> source
        config.NewConfig<Tea, Dto.Teas.Tea>()
            .Map(m => m.PriceUSD, s => s.Price)
            .Map(m => m.PriceBTC, s => s.Price * BtcRate);
    }
}