using System.ComponentModel.DataAnnotations;

namespace ZedLive.Domain.ValueObjects;

public sealed class EmailObject(string value) : ValueObjects
{
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string Value { get; set; } = value;

    public override string ToString()
    {
        return Value;
    }
}