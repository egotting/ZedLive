namespace ZedLive.Domain.ValueObjects;

public class Error : ValueObjects
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null");

    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public string Code { get; }
    public string Description { get; }
    public static implicit operator string(Error error) => error.Code;
}