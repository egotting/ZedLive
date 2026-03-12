using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ZedLive.Application.CQRS.User.Command.Login;
using ZedLive.Application.CQRS.User.Command.Register;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class User : ControllerBase
{
    private readonly ISender _sender;

    public User(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        Result result = null;
        try
        {
            result = await _sender.Send(command);
            return Ok(result);
        }
        catch (Exception _)
        {
            return BadRequest(result);
        }
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> Login(LoginUserCommand command)
    {
        Result result = null;
        try
        {
            result = await _sender.Send(command);
            return Ok(result);
        }
        catch (Exception _)
        {
            return BadRequest(result);
        }
    }
}