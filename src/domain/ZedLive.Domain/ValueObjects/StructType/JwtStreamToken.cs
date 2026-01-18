using System.Globalization;

namespace ZedLive.Domain.ValueObjects.StructType;

public record JwtStreamToken
{
    public string SecretKey { get; set; } = string.Empty;
    public string IssuedAt { get; set; } = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
    public string StreamKey { get; set; } = string.Empty;
}