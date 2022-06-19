using JuniorTemplate.Application.Models.Teas;
using JuniorTemplate.Data.Entities;
using Mapster;

namespace JuniorTemplate.Application;

public class MapperRegister : IRegister
{
	private const decimal BTCRate = 0.000034m;

	public void Register(TypeAdapterConfig config)
	{
		// m -> member
		// s -> source
		config.NewConfig<Tea, TeaModel>()
				.Map(m => m.PriceUSD, s => s.Price)
				.Map(m => m.PriceBTC, s => s.Price * BTCRate);
	}
}