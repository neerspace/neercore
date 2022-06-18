using MediatR;

namespace SeniorTemplate.Application.Features.Teas.Models;

public class TeaByIdQuery : IRequest<TeaModel>
{
	public Guid Id { get; }

	public TeaByIdQuery(Guid id) => Id = id;
}