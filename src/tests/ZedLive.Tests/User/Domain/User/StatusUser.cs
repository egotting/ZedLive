namespace ZedLive.Tests.User.Domain.User;

public class StatusUser : ValueObjects.ValueObjects
{
    public StatusUser()
    {
    }

    public StatusUser(string value)
    {
        Value = value;
    }

    public static StatusUser Online => new(nameof(Online));

    public static StatusUser Offline => new(nameof(Offline));

    public static StatusUser Inactive => new(nameof(Inactive));

    public string Value { get; private set; } = null!;
}