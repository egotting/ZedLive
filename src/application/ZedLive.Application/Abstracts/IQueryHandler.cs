using MediatR;
using ZedLive.Application.Abstracts;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Application.Services.Abstracts;

public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>> where TQuery 
    : IQuery<TResponse>
{
}