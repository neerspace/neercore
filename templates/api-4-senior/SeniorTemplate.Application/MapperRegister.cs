using Mapster;
using SeniorTemplate.Data.Entities;

namespace SeniorTemplate.Application;

public class MapperRegister : IRegister
{
	private const decimal BTCRate = 0.000034m;

	public void Register(TypeAdapterConfig config)
	{
		// m -> member
		// s -> source
		config.NewConfig<Tea, Features.Teas.Models.TeaModel>()
				.Map(m => m.PriceUSD, s => s.Price)
				.Map(m => m.PriceBTC, s => s.Price * BTCRate);
	}
}