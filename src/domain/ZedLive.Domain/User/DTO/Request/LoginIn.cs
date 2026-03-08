namespace ZedLive.Domain.User.DTO.Request;

public record LoginIn(string login, string email, string password);