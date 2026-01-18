namespace ZedLive.Domain.User;

public class StatusUser(string value) : ValueObjects.ValueObjects
{
    public static StatusUser Online => new StatusUser(nameof(Online));

    public static StatusUser Offline => new StatusUser(nameof(Offline));

    public static StatusUser Inactive => new StatusUser(nameof(Inactive));

    public string Value { get; } = value;
}