using ZedLive.Domain.ValueObjects;

namespace ZedLive.Domain.User.DTO.Request;

public record RegisterIn(string? login, EmailObject? email, PasswordObject password, string confirmPassword);