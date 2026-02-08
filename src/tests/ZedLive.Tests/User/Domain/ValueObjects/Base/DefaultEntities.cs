namespace ZedLive.Tests.User.Domain.ValueObjects.Base;

public abstract class DefaultEntities
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LoggedAt { get; set; }
}