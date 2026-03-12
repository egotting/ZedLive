using ZedLive.Application.Abstracts;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Application.CQRS.User.Command.Register;

public record RegisterUserCommand(string? login, string email, string password, string confirmPassword)
    : ICommand;