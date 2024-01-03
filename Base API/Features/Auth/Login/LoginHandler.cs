using BaseAPI.Common.Extensions;
using BaseAPI.Common.Settings;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using static BaseAPI.Common.Constants.Auth;

namespace BaseAPI.Features.Auth.Login;

public static class LoginHandler
{
    public static async Task<IResult> Handle(LoginRequest request, [FromServices] ILoginService loginService)
    {
        var result = await loginService.LoginAsync(request);
        return result.AsHttpResponse();
    }

    public static async Task<IResult> HandleWeb(
        LoginRequest request, 
        HttpContext http,
        [FromServices] IOptions<AppSettings> appSettings,
        [FromServices] IOptions<JwtSettings> jwtSettings,
        [FromServices] ILoginService loginService
    )
    {
        var result = await loginService.LoginAsync(request);
        var loginResult = result.AsHttpResponse();

        if (loginResult is not Ok<LoginResponse> okResult)
            return loginResult;

        LoginResponse loginResponse = okResult.Value!;
        http.Response.Cookies.Append(TokenTypes.RefreshToken, loginResponse.RefreshToken, new CookieOptions()
        {
             Domain = appSettings.Value.Domain,
             Expires = DateTimeOffset.UtcNow.AddDays(jwtSettings.Value.RefreshTokenExpiryInDays),
             HttpOnly = true,
             Path = "/",
             Secure = true,
             SameSite = SameSiteMode.Strict
        });

        return Results.Ok(new LoginWebResponse(loginResponse.AccessToken));
    }
}
