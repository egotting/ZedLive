using System.ComponentModel.DataAnnotations;

namespace ZedLive.Domain.ValueObjects;

public abstract class EmailObject : ValueObjects
{
    [EmailAddress] private string Value { get; set; }

    protected EmailObject(string value)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Invalid Email");
        Value = value;
    }
}