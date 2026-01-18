using ZedLive.Domain.ValueObjects.StructType;

namespace ZedLive.Domain.Contracts.Users.Configuration;

public interface IStreamKey
{
    public ValueTask<string> GenerateToken();
    public Task ValidateToken();
}