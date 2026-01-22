using ZedLive.Domain.ValueObjects.StructType;

namespace ZedLive.Domain.Contracts.Users.Configuration;

public interface IStreamKeyService
{
    public string GenerateStreamKey(User.User user);
}