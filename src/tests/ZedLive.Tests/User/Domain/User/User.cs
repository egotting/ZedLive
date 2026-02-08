using ZedLive.Tests.User.Domain.ValueObjects;
using ZedLive.Tests.User.Domain.ValueObjects.Base;

namespace ZedLive.Tests.User.Domain.User;

public class User : DefaultEntities
{
    public string Login { get; set; } = string.Empty;
    public EmailObject Email { get; set; } = null!;
    public PasswordObject Password { get; set; } = null!;
    public IEnumerable<StatusUser> Status { get; set; } = null!;

    public string StreamKey { get; set; } = string.Empty;

#region constrctor to EF CORE

    private User()
    {
        // EF CORE
    }

#endregion


    public static User CreateUser(string login, EmailObject email, PasswordObject password)
        => new User(login, email, password, StatusUser.Online, string.Empty);

    public User(string login, EmailObject email, PasswordObject password, StatusUser statusUser, string streamKey)
    {
        Login = login;
        Email = email;
        Password = password;
        Status = [statusUser];
        StreamKey = streamKey;
    }
}