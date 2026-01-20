using System.Security.Cryptography;
using System.Text;

namespace ZedLive.Tests.ValueObjects;

public class PasswordObject
{
    // TESTE HASHPASSOWORD
    [Fact]
    public void HashPasswordTest()
    {
        const int SaltSize = 16;
        const int HashSize = 32;
        const int Interations = 100000;
        const string password = "password123";
        HashAlgorithmName Algorith = HashAlgorithmName.SHA512;

        
        // create
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Interations, Algorith, HashSize);
        
        // verify
        string passwordHash = $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        string[] parts = passwordHash.Split("-");
        byte[] hashVerify = Convert.FromHexString(parts[0]);
        byte[] saltVerify = Convert.FromHexString(parts[1]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, saltVerify, Interations, Algorith, HashSize);

        var verify = CryptographicOperations.FixedTimeEquals(hashVerify, inputHash);
        Assert.True(verify);
    }
}