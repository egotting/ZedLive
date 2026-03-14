using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ZedLive.Application.CQRS.User.Command.Login;
using ZedLive.Application.CQRS.User.Command.Register;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class Auth(ISender _sender) : ControllerBase
{
    [HttpPost("sign-up")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
        => Ok(await _sender.Send(command));

    [HttpPost("sign-in")]
    public async Task<IActionResult> Login(LoginUserCommand command)
        => Ok(await _sender.Send(command));
}