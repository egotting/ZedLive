using Microsoft.Extensions.Configuration;
using ZedLive.Domain.Contracts.Users.Configuration;

namespace ZedLive.Application.Services.Stream;

public class StreamKey(IConfiguration configuration) : IStreamKey
{
    private readonly IConfiguration _configuration = configuration;

    public ValueTask<string> GenerateToken()
    {
        throw new NotImplementedException();
    }

    public Task ValidateToken()
    {
        throw new NotImplementedException();
    }
}