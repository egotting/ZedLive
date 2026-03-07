namespace ZedLive.Domain.User.DTO.Request;

public record LoginReq(string login, string email, string password);