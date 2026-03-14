using ZedLive.Application.Abstracts;
using ZedLive.Domain.User;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Application.CQRS.User.Query.ListStatus;

public record ListStatusQuery : IQueryList<StatusUser>;