using System.Security.Claims;

namespace ZedLive.Tests.User.Domain.Contracts.Users.Configuration;

public interface IJwt
{
    public string Generate(ZedLive.Tests.User.Domain.User.User user);
    public string GenerateRefreshToken();
    public ClaimsPrincipal? ValidateToken(string token, bool validateLifetime = true);
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}