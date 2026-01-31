using Microsoft.Extensions.DependencyInjection;
using ZedLive.Application.Services.Jwt;
using ZedLive.Application.Services.Stream;
using ZedLive.Domain.Contracts.Users.Configuration;

namespace ZedLive.IoC.Services;

public static class ServiceInjection
{
    public static IServiceCollection AddInjection(this IServiceCollection service)
    {
        service.AddScoped<IJwtStream, JwtStream>();
        service.AddScoped<IJwt, Jwt>();

        return service;
    }
}