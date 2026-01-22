namespace ZedLive.Domain.ValueObjects.StructType;

public record JwtOptions
{
    // WARNING: THIS FILE CANNOT BE SHOWING TO ANYONE
    public string SecretKey { get; set; } = "</zM+u:oCSiGgw,l@$[}n£NQ837h=#,dO:AtegMA&%";
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int AccessTokenExpirationMinutes { get; set; } = 15;
    public int RefreshTokenExpirationDays { get; set; } = 7;
}