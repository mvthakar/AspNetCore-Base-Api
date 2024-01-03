using System.Security.Claims;

using BaseAPI.Common.Extensions;
using BaseAPI.Common.Settings;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using static BaseAPI.Common.Constants.Auth;

namespace BaseAPI.Features.Auth.Token.Refresh;

public static class RefreshTokenHandler
{
    public static async Task<IResult> Handle(
        RefreshTokenRequest request,
        ClaimsPrincipal claims,
        [FromServices] IRefreshTokenService refreshTokenService
    )
    {
        var result = await refreshTokenService.RefreshTokenAsync(request, claims.GetUserId());
        return result.AsHttpResponse();
    }

    public static async Task<IResult> HandleWeb(
        RefreshTokenRequest request,
        ClaimsPrincipal claims,
        HttpContext http,
        [FromServices] IOptions<AppSettings> appSettings,
        [FromServices] IOptions<JwtSettings> jwtSettings,
        [FromServices] IRefreshTokenService refreshTokenService
    )
    {
        var serviceResult = await refreshTokenService.RefreshTokenAsync(request, claims.GetUserId());
        var result = serviceResult.AsHttpResponse();

        if (result is not Ok<RefreshTokenResponse> okResult)
            return result;

        RefreshTokenResponse response = okResult.Value!;
        http.Response.Cookies.Append(TokenTypes.RefreshToken, response.RefreshToken, new CookieOptions()
        {
            Domain = appSettings.Value.Domain,
            Expires = DateTimeOffset.UtcNow.AddDays(jwtSettings.Value.RefreshTokenExpiryInDays),
            HttpOnly = true,
            Path = "/",
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        return Results.Ok(new RefreshTokenWebResponse(response.AccessToken));
    }
}
