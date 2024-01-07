using System.Security.Claims;

using BaseAPI.Common.Extensions;
using BaseAPI.Common.Utilities;

using Microsoft.AspNetCore.Mvc;

using static BaseAPI.Common.Constants.Auth;

namespace BaseAPI.Features.Auth.Logout;

public static class LogoutHandler
{
    public static async Task<IResult> HandleLogout(LogoutRequest request, ClaimsPrincipal claims, [FromServices] ILogoutService logoutService)
    {
        var result = await logoutService.LogoutAsync(request, claims.GetUserId());
        return result.AsHttpResponse();
    }

    public static async Task<IResult> HandleLogoutWeb(HttpContext http, [FromServices] ILogoutService logoutService)
    {
        var refreshToken = http.Request.Cookies[TokenTypes.RefreshToken];
        if (refreshToken is null)
            return Results.Unauthorized();

        return await HandleLogout(new LogoutRequest() { RefreshToken = refreshToken }, http.User, logoutService);
    }

    public static async Task<IResult> HandleLogoutAll(ClaimsPrincipal claims, [FromServices] ILogoutService logoutService)
    {
        var result = await logoutService.LogoutAllAsync(claims.GetUserId());
        if (!result.IsSuccess)
        {
            return result.Error switch
            {
                NotFoundError => Results.Unauthorized(),
                _ => Results.Problem()
            };
        }

        return Results.Ok();
    }
}
