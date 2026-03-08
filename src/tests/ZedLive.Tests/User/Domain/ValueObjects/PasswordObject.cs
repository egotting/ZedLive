using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace ZedLive.Tests.User.Domain.ValueObjects;

public class PasswordObject : ValueObjects
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Interations = 100000;

    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
    private readonly ILogger<PasswordObject> _logger;

    private string Value { get; set; }

    public PasswordObject(string value)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Password Invalid");
        Value = value.Length switch
        {
            < 1 => throw new ArgumentException("Need until 8 characters"),
            > 8 => throw new ArgumentException("Cannot pass the 8 characters"),
            _ => value
        };
    }

    public static (string password, string salt) Hash(string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException("Need put a value");
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(value, salt, Interations, Algorithm, HashSize);
        return (Convert.ToHexString(hash), Convert.ToHexString(salt));
    }

    public static bool Verify(string value, string salt, string password)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Need put a value");
        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] encrypt = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Interations, Algorithm, HashSize);


        string passwordDb = $"{Convert.FromBase64String(value)}{Convert.FromBase64String(salt)}";
        string? passwordLogin = Convert.ToString(encrypt);
        return String.Equals(passwordLogin, passwordDb);
    }
}