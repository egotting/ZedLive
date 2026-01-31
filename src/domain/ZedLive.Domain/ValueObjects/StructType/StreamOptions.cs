using System.Globalization;

namespace ZedLive.Domain.ValueObjects.StructType;

public record StreamOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public string TimeTokenIsCreated { get; set; } = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
}