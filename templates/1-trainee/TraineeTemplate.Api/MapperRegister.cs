using Mapster;
using TraineeTemplate.Api.Data.Entities;

namespace TraineeTemplate.Api;

public class MapperRegister : IRegister
{
	private readonly decimal _btcRate = 0.000034m;

	public void Register(TypeAdapterConfig config)
	{
		// m -> member
		// s -> source
		config.NewConfig<Tea, Dto.Tea>()
				.Map(m => m.PriceUSD, s => s.Price)
				.Map(m => m.PriceBTC, s => s.Price * _btcRate);
	}
}