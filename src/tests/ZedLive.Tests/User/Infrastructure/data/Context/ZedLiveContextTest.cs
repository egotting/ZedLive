using Microsoft.EntityFrameworkCore;
using Npgsql;
using ZedLive.Infrastructure.Data.Context;

namespace ZedLive.Tests.User.Infrastructure.data.Context;

public sealed class ZedLiveContextTest
{
    private const string _connectionString =
        "User ID=postgres;Password=postgres123;Host=localhost;Port=5555;Database=ZedLiveDatabaseTest;Pooling=true;Connection Lifetime=0;";

    public ZedLiveContext CreateContext()
    {
        try
        {
            var options = new DbContextOptionsBuilder<ZedLiveContext>()
                .UseNpgsql(_connectionString,
                    x =>
                    {
                        // x.EnableRetryOnFailure();
                        x.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                    })
                .Options;

            var context = new ZedLiveContext(options);
            context.Database.EnsureCreated();
            return context;
        }
        catch (Exception e)
        {
            throw new Exception("DEU ERR" + e);
        }
    }
}