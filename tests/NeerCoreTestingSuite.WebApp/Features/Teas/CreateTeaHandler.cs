using Mapster;
using NeerCore.Application.Abstractions;
using NeerCoreTestingSuite.WebApp.Dto.Teas;
using NeerCoreTestingSuite.WebApp.Services;

namespace NeerCoreTestingSuite.WebApp.Features.Teas;

public class CreateTeaHandler : ICommandHandler<CreateTeaCommand, Tea>
{
	private readonly TeasService _teasService;
	public CreateTeaHandler(TeasService teasService) => _teasService = teasService;

	public async Task<Tea> Handle(CreateTeaCommand request, CancellationToken cancellationToken)
	{
		var entity = request.Adapt<Data.Entities.Tea>();
		await _teasService.CreateAsync(entity);
		return entity.Adapt<Tea>();
	}
}