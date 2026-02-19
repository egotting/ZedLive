using MediatR;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Application.Abstracts;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>
{
}