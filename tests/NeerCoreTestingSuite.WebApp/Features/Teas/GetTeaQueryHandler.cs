using Mapster;
using NeerCore.Application.Abstractions;
using NeerCoreTestingSuite.WebApp.Dto.Teas;
using NeerCoreTestingSuite.WebApp.Services;

namespace NeerCoreTestingSuite.WebApp.Features.Teas;

public class GetTeaQueryHandler : IQueryHandler<GetTeaQuery, Tea>
{
	private readonly TeasService _teasService;
	public GetTeaQueryHandler(TeasService teasService) => _teasService = teasService;

	public async Task<Tea> Handle(GetTeaQuery request, CancellationToken cancellationToken)
	{
		var entity = await _teasService.GetByIdAsync(request.Id);
		return entity.Adapt<Tea>();
	}
}