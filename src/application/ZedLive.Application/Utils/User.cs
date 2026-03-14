using System.Security.Claims;

namespace ZedLive.Application.Utils;

internal static class User
{
    public static int GetUserId(this ClaimsPrincipal claims)
    {
        int id;
        var claimUser = claims.FindFirst("uid");
        int.TryParse(claimUser?.Value, out id);
        return id;
    }
}