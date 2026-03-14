using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZedLive.Application.Services.Stream;
using ZedLive.Domain.Contracts.Users.Configuration;
using ZedLive.Domain.User;
using ZedLive.Domain.ValueObjects.StructType;

namespace ZedLive.Application.Services.Jwt;

public sealed class Jwt(IOptions<JwtOptions> opt, ILogger<Jwt> _logger) : IJwt
{
    private readonly JwtOptions _jwtOptions = opt.Value;

    public string Generate(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Login),
            new(ClaimTypes.Email, user.Email.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

        var tokenDecriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_jwtOptions.AccessTokenExpirationHours),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var token = tokenHandler.CreateToken(tokenDecriptor);
        var tokenConvert = tokenHandler.WriteToken(token);
        _logger.LogInformation("token generated for user {Username}, {tokenConvert}", user.Email, tokenConvert);
        return tokenConvert;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? ValidateToken(string token, bool validateLifetime = true)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);

        try
        {
            var validateParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtOptions.Audience,
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.FromMinutes(1),
            };
            var principal = tokenHandler.ValidateToken(token, validateParameters, out var validatedToken);

            if (validatedToken is JwtSecurityToken jwtToken &&
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogWarning("Token validated failed: invalid algorithm {Algorithm}", jwtToken.Header.Alg);
                return null;
            }

            return principal;
        }
        catch (SecurityTokenException e)
        {
            _logger.LogWarning(e, "Token validated failed");
            return null;
        }
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        return ValidateToken(token, false);
    }
}