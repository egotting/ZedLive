using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ZedLive.Domain.ValueObjects;

public sealed class EmailObject : ValueObjects
{
    public string Value { get; }

    public EmailObject(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be null or empty");

        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email");

        Value = value;
    }

    private static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}