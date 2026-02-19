using System.Security.Cryptography;

namespace ZedLive.Domain.ValueObjects;

public sealed class PasswordObject : ValueObjects
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Interations = 100000;

    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public string Value { get; set; }

    public PasswordObject(string value)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Password Invalid");
        Value = value.Length switch
        {
            < 1 => throw new ArgumentException("Need until 8 characters"),
            > 8 => throw new ArgumentException("Cannot pass the 8 characters"),
            _ => Hash(value)
        };
    }

    private static string Hash(string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException("Need put a value");
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(value, salt, Interations, Algorithm, HashSize);
        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    public static bool Verify(string value, string valueHashed)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(valueHashed))
            throw new ArgumentException("Need put a value");
        string[] parts = valueHashed.Split("-");

        byte[] hash = Convert.FromHexString(parts[0]);
        byte[] salt = Convert.FromHexString(parts[1]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(value, salt, Interations, Algorithm, HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}