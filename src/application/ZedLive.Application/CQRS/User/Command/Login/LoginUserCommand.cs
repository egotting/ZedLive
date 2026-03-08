using MediatR;
using ZedLive.Application.Abstracts;
using ZedLive.Domain.User.DTO.Request;
using ZedLive.Domain.User.DTO.Response;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Application.CQRS.User.Command.Login;

public sealed record LoginUserCommand(
    string login,
    string email,
    string password) : ICommand<string>;