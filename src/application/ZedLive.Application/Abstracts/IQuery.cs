using MediatR;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Application.Abstracts;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}