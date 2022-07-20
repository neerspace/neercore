using MediatR;

namespace NeerCore.Application.Abstractions;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
		where TQuery : IQuery<TResponse> { }