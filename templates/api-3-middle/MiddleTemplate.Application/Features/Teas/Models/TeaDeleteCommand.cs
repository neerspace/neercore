using MediatR;

namespace MiddleTemplate.Application.Features.Teas.Models;

public struct TeaDeleteCommand : IRequest
{
	public Guid Id { get; }

	public TeaDeleteCommand(Guid id) => Id = id;
}