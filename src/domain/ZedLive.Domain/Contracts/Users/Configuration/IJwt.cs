using System.Security.Claims;

namespace ZedLive.Domain.Contracts.Users.Configuration;

public interface IJwt
{
    public string Generate(User.User user);
    public string GenerateRefreshToken();
    public ClaimsPrincipal? ValidateToken(string token, bool validateLifetime = true);
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}