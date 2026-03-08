using MediatR;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Application.Abstracts;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;