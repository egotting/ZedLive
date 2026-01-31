namespace ZedLive.Domain.Contracts.Users.Configuration;

public interface IJwtStream
{
    public string Generate(User.User user);
}