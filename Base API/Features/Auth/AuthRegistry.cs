using BaseAPI.Common;
using BaseAPI.Common.Utilities;
using BaseAPI.Features.Auth.Login;
using BaseAPI.Features.Auth.Logout;
using BaseAPI.Features.Auth.SignUp;
using BaseAPI.Features.Auth.Token;
using BaseAPI.Features.Auth.Token.Refresh;

using FluentValidation;

using static BaseAPI.Common.Constants.Auth;

namespace BaseAPI.Features.Auth;

public class AuthRegistry : IRegistry
{
    public void MapEndpoints(WebApplication app)
    {
        app.MapPost(Api.Url("auth/signup"), SignUpHandler.Handle);
        app.MapPost(Api.Url("auth/login"), LoginHandler.Handle);
        app.MapPost(Api.Url("auth/login-web"), LoginHandler.HandleWeb);

        app.MapPost(Api.Url("auth/token/refresh"), RefreshTokenHandler.Handle).RequireAuthorization(Jwt.AllowExpired);
        app.MapPost(Api.Url("auth/token/refresh-web"), RefreshTokenHandler.HandleWeb).RequireAuthorization(Jwt.AllowExpired);

        app.MapPost(Api.Url("auth/logout"), LogoutHandler.HandleLogout).RequireAuthorization(Jwt.AllowExpired);
        app.MapPost(Api.Url("auth/logout-web"), LogoutHandler.HandleLogoutWeb).RequireAuthorization(Jwt.AllowExpired);
        
        app.MapPost(Api.Url("auth/logout-all"), LogoutHandler.HandleLogoutAll).RequireAuthorization(Jwt.AllowExpired);
    }

    public void AddServices(IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IValidator<RefreshTokenRequest>, RefreshTokenRequestValidator>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
        services.AddScoped<ILoginService, LoginService>();

        services.AddScoped<IValidator<SignUpRequest>, SignUpRequestValidator>();
        services.AddScoped<ISignUpService, SignUpService>();

        services.AddScoped<IValidator<LogoutRequest>, LogoutRequestValidator>();
        services.AddScoped<ILogoutService, LogoutService>();
    }
}
