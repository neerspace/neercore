using MediatR;

namespace NeerCore.Application.Abstractions;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
		where TCommand : ICommand<TResponse> { }

public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
		where TCommand : ICommand<Unit> { }