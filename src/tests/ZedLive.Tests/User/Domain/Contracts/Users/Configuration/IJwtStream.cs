namespace ZedLive.Tests.User.Domain.Contracts.Users.Configuration;

public interface IJwtStream
{
    public string Generate(ZedLive.Tests.User.Domain.User.User user);
}