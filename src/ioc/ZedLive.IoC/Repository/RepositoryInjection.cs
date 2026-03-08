using Microsoft.Extensions.DependencyInjection;
using ZedLive.Domain.Contracts.Users.Infra;
using ZedLive.Infrastructure.Repository.User;

namespace ZedLive.IoC.Repository;

public static class RepositoryInjection
{
    public static void AddRepositories(this IServiceCollection service)
    {
        service.AddScoped<IUnitOfWork, UnitOfWork>();
        service.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
    }
}