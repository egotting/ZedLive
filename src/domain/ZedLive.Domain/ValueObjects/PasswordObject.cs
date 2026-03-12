using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace ZedLive.Domain.ValueObjects;

public sealed class PasswordObject : ValueObjects
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Interations = 100000;

    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
    public string Value { get; }

    public PasswordObject(string value)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Password Invalid");
        Value = value;
    }

    public static (string password, byte[] salt) Hash(string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException("Need put a value");
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(value, salt, Interations, Algorithm, HashSize);
        return (Convert.ToHexString(hash), salt);
    }

    public static bool Verify(string value, byte[] salt, string password)
    {
        if (String.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Need put a value");
        byte[] encrypt = Rfc2898DeriveBytes.Pbkdf2(password, salt, Interations, Algorithm, HashSize);
        var loginPw = Convert.ToHexString(encrypt);
        return (string.Equals(loginPw, value, StringComparison.InvariantCultureIgnoreCase));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}