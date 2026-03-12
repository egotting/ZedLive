using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZedLive.Domain.Contracts.Users.Configuration;
using ZedLive.Domain.User;
using ZedLive.Domain.ValueObjects.StructType;

namespace ZedLive.Application.Services.Stream;

public sealed class JwtStream(IOptions<StreamOptions> options, ILogger<JwtStream> logger) : IJwtStream
{
    private readonly StreamOptions _streamOptions = options.Value;

    public string Generate(Domain.User.User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var keyPass = CreateKey();
        var key = Encoding.ASCII.GetBytes(_streamOptions.SecretKey);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email.Value),
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
        logger.LogInformation("Stream Key generated for user {Username}", user.Id);
        return $"{tokenConvert}-${keyPass}";
    }


    private static string CreateKey()
    {
        var random = new Random();
        const string str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        const int size = 6;
        var value = "";
        for (var i = 0; i < size; i++)
        {
            var x = random.Next(26);
            value = value + str[x];
        }

        return value;
    }
}