using ZedLive.Domain.ValueObjects;
using ZedLive.Domain.ValueObjects.Base;

namespace ZedLive.Domain.User;

public class User : DefaultEntities
{
    public string Login { get; set; } = string.Empty;
    public EmailObject Email { get; set; }
    public PasswordObject Password { get; set; }
    public IEnumerable<StatusUser> Status { get; set; }

    private StreamKeyType StreamKey { get; set; }

#region constrctor to EF CORE

    public User()
    {
        // EF CORE
    }

#endregion


    public static User CreateUser(string login, EmailObject email, PasswordObject password)
        => new User(login, email, password, StatusUser.Online);

    public User(string login, EmailObject email, PasswordObject password, StatusUser statusUser)
    {
        Login = login;
        Email = email;
        Password = password;
        Status = [statusUser];
    }
}