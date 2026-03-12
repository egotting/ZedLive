namespace ZedLive.Domain.ValueObjects;

public class Error
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null");
    public static readonly Error InvalidPassword = new("Error.InvalidPassword", "Password is not correct");
    public static readonly Error IncorrectPassword = new("Error.IncorrectPassword", "Password must be the same");
    public static readonly Error AlreadyRegister = new("Error.AlreadyRegister", "This user already register");

    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public string Code { get; }
    public string Description { get; }
    public static implicit operator string(Error error) => error.Code;
}