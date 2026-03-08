using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZedLive.Domain.ValueObjects.StructType;
using ZedLive.Infrastructure.Data.Context;

namespace ZedLive.IoC.Database;

public static class DatabaseInjection
{
    internal static void AddInfrastructure(this IServiceCollection service,
        IConfiguration configuration)
    {
        service.AddDbContext<ZedLiveContext>(opt =>
            {
                opt.EnableDetailedErrors();
                opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                opt.UseNpgsql(configuration.GetConnectionString("DbConnection"),
                    b => b.MigrationsAssembly("ZedLive.Infrastructure")).LogTo(Console.WriteLine, LogLevel.Information);
            },
            ServiceLifetime.Scoped);
    }
}