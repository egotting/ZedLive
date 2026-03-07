namespace ZedLive.Domain.User.DTO.Request;

public record RegisterReq(string? login, string? email, string password, string confirmPassword);