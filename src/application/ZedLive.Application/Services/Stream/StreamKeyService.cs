using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ZedLive.Domain.Contracts.Users.Configuration;
using ZedLive.Domain.User;
using ZedLive.Domain.ValueObjects.StructType;

namespace ZedLive.Application.Services.Stream;

public class StreamKeyService(StreamOptions _streamOptions, ILogger<StreamKeyService> _logger) : IStreamKeyService
{
    public string GenerateStreamKey(User user)
    {
        var keyPass = CreateKey();
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_streamOptions.SecretKey);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email.ToString()),
            new(JwtRegisteredClaimNames.Iat, _streamOptions.TimeTokenIsCreated),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Jti, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer32)
        };

        var tokenDecriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = user.Email.ToString(),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var token = tokenHandler.CreateToken(tokenDecriptor);
        var tokenConvert = tokenHandler.WriteToken(token);
        _logger.LogInformation("Stream Key generated for user {Username}", user.Email);
        return $"{tokenConvert}-${keyPass}";
    }

    private string CreateKey()
    {
        var random = new Random();
        var str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        int size = 6;
        var value = "";
        for (int i = 0; i < size; i++)
        {
            int x = random.Next(26);
            value = value + str[x];
        }

        return value;
    }
}