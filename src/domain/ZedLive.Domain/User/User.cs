using ZedLive.Domain.ValueObjects;
using ZedLive.Domain.ValueObjects.Base;

namespace ZedLive.Domain.User;

public class User : DefaultEntities
{
    public string? Login { get; set; } = string.Empty;
    public EmailObject Email { get; set; }
    public PasswordObject Password { get; set; }
    public byte[] Salt { get; set; } = [];
    public byte StatusUserId { get; set; }

    public string StreamKey { get; set; }
    public StatusUser StatusUser { get; set; }

    #region constrctor to EF CORE

    private User()
    {
        // EF CORE
    }

    #endregion


    public static User CreateUser(string? login, string email, string password, byte[] salt, string streamKey)
        => new User(login, new EmailObject(email), new PasswordObject(password), salt, streamKey);

    private User(string? login, EmailObject email, PasswordObject password, byte[] salt,
        string streamKey)
    {
        Login = login;
        Email = email;
        Password = password;
        Salt = salt;
        StreamKey = streamKey;
    }
}