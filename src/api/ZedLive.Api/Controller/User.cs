using Microsoft.AspNetCore.Mvc;

namespace ZedLive.Api.Controller;

[ApiController]
[Route("test")]
public class User
{
    [HttpGet]
    public string HelloWorld()
    {
        return "dadaa";
    }
}