using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ZedLive.Domain.ValueObjects.StructType;
using ZedLive.Infrastructure.Data.Context;

namespace ZedLive.IoC.Database;

public static class DatabaseInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service,
        string connectionStrings)
    {
        service.AddDbContext<ZedLiveContext>(opt =>
            {
                opt.EnableDetailedErrors();
                opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                opt.UseNpgsql(connectionStrings);
            },
            ServiceLifetime.Scoped);
        return service;
    }
}