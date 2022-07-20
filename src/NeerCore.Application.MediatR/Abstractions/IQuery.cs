using MediatR;

namespace NeerCore.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse> { }