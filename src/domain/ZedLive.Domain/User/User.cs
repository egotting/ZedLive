using ZedLive.Domain.ValueObjects;
using ZedLive.Domain.ValueObjects.Base;

namespace ZedLive.Domain.User;

public class User : DefaultEntities
{
    public string? Login { get; set; } = string.Empty;
    public EmailObject Email { get; set; }
    public PasswordObject Password { get; set; }
    public string Salt { get; set; } = string.Empty;

    public IEnumerable<StatusUser> Status { get; set; }

    public string StreamKey { get; set; }

    #region constrctor to EF CORE
    private User()
    {
        // EF CORE
    }
    #endregion


    public static User CreateUser(string login, EmailObject email, PasswordObject password, string salt)
        => new User(login, email, password, salt,StatusUser.Online);

    public User(string login, EmailObject email, PasswordObject password, string salt, StatusUser statusUser)
    {
        Login = login;
        Email = email;
        Password = password;
        Salt = salt;
        Status = [statusUser];
    }
}