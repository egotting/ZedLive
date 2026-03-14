using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZedLive.Application.CQRS.User.Query.ListStatus;
using ZedLive.Domain.User;
using ZedLive.Domain.ValueObjects;

namespace ZedLive.Api.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class User(ISender _sender) : ControllerBase
{
    [HttpGet("/status")]
    public async Task<IActionResult> GetAllStatus([FromQuery] ListStatusQuery query)
        => Ok(await _sender.Send(query));
}