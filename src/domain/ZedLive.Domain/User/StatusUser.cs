namespace ZedLive.Domain.User;

public class StatusUser(string value) : ValueObjects.ValueObjects
{
    public static StatusUser Online => new(nameof(Online));

    public static StatusUser Offline => new(nameof(Offline));

    public static StatusUser Inactive => new(nameof(Inactive));

    public string Value { get; } = value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}