using System.ComponentModel.DataAnnotations;

namespace ZedLive.Domain.ValueObjects;

public sealed class EmailObject : ValueObjects
{
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string? Value { get; set; }

    public EmailObject(string value)
    {
        Value = value;
    }

    public override string? ToString()
    {
        return Value;
    }
}