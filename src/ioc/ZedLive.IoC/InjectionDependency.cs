using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZedLive.Application;
using ZedLive.IoC.Database;
using ZedLive.IoC.Repository;
using ZedLive.IoC.Services;
using MediatR;

namespace ZedLive.IoC;

public static class InjectionDependency
{
    public static void DependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddServices();
        services.AddRepositories();
        services.AddMediatR(AssemblyReference.Assembly);
    }
}