using MediatR;

namespace NeerCore.Application.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse> { }

public interface ICommand : ICommand<Unit> { }