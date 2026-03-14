using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using ZedLive.Application.Abstracts;
using ZedLive.Domain.Contracts.Users.Infra;
using ZedLive.Domain.User;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Application.CQRS.User.Query.ListStatus;

public class ListStatusQueryHandler(IUnitOfWork _unit, ILogger<ListStatusQueryHandler> _logger)
    : IQueryListHandler<ListStatusQuery, StatusUser>
{
    public async Task<Result<IEnumerable<StatusUser>>> Handle(ListStatusQuery request,
        CancellationToken cancellationToken)
    {
        var status = await _unit.Repository<StatusUser>()
            .GetAllAsync(
                0,
                3,
                cancellationToken);
        return Result.Success(status);
    }
}