using System.Windows.Input;
using MediatR;
using ZedLive.Application.Abstracts;
using ZedLive.Domain.User.DTO.Request;
using ZedLive.Domain.User.DTO.Response;

namespace ZedLive.Application.CQRS.User.Command;

public sealed record LoginUserCommand(LoginIn req) : ICommand<LoginOut>;    