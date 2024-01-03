using System.Security.Claims;

using BaseAPI.Common.Constants;

namespace BaseAPI.Common.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static long GetUserId(this ClaimsPrincipal claims)
    {
        string id = claims.FindFirstValue(Auth.Claims.UserId)!;
        return long.Parse(id);
    }
}
