using System.ComponentModel.DataAnnotations;

namespace ZedLive.Tests.User.Domain.ValueObjects;

public sealed class EmailObject : ValueObjects
{
    [EmailAddress] private string Value { get; set; }

    public EmailObject(string value)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Invalid Email");
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}