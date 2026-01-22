using System.Globalization;

namespace ZedLive.Domain.ValueObjects.StructType;

public record StreamOptions
{
    // WARNING: THIS FILE CANNOT BE SHOWING TO ANYONE
    public string SecretKey { get; set; } = "c>7+0[b+78CN2:eS|Q=9M5Oez?0}g-X-V.$";
    public string TimeTokenIsCreated { get; set; } = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
}