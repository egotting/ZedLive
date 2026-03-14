using MediatR;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Application.Abstracts;

public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>> where TQuery
    : IQuery<TResponse>;

public interface IQueryListHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, Result<IEnumerable<TResponse>>> where TQuery
    : IQueryList<TResponse>;