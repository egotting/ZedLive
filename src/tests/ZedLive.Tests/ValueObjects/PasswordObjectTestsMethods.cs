using System.Security.Cryptography;
using System.Text;
using Moq;
using ZedLive.Tests.User.Domain.ValueObjects;

namespace ZedLive.Tests.ValueObjects;

public class PasswordObjectTestsMethods
{
    private readonly Mock<PasswordObject> _mock;

    [Fact]
    public void HashPasswordTest()
    {
        const string password = "password123";
        _mock.Setup(x => PasswordObject.Hash(password));
    }
}