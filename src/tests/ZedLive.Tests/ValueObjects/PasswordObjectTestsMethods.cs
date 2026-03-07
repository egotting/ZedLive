using System.Security.Cryptography;
using System.Text;
using Moq;

namespace ZedLive.Tests.ValueObjects;

public class PasswordObject
{
    private readonly Mock<User.Domain.ValueObjects.PasswordObject> _mock;
    
    // TESTE HASHPASSOWORD
    [Fact]
    public void HashPasswordTest()
    {
        const string password = "password123";
        
        _mock.Setup(x => x.);
        
    }
}