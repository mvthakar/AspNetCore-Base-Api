using BaseAPI.Common.Constants;
using BaseAPI.Common.Utilities;
using BaseAPI.Database;
using BaseAPI.Database.Models.Identity;
using BaseAPI.Features.Auth.Token;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using static BaseAPI.Common.Constants.Auth;

namespace BaseAPI.Features.Auth.Login;

public interface ILoginService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
}

public class LoginService(
    IValidator<LoginRequest> validator,
    ITokenService tokenService,
    DatabaseContext database,
    UserManager<User> userManager,
    SignInManager<User> signInManager
) : ILoginService
{
    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        ValidationResult validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Fail(Errors.Validation.WithResult(validationResult));

        var foundUser = await userManager.FindByEmailAsync(request.Email);
        if (foundUser is null || await userManager.IsLockedOutAsync(foundUser))
            return Result.Fail(Errors.Unauthorized.WithMessage("Wrong email or password"));

        if (!await userManager.IsEmailConfirmedAsync(foundUser))
            return Result.Fail(Errors.Unauthorized.WithMessage("Email is not verified"));

        var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
        if (!result.Succeeded)
            return Result.Fail(Errors.Unauthorized.WithMessage("Wrong email or password"));

        var authProvider = await database.AuthProviders.FirstAsync(provider => provider.Name == Providers.Email);
        var tokenType = await database.TokenTypes.FirstAsync(tokenType => tokenType.Name == TokenTypes.RefreshToken);

        var tokenInDatabase = await database.UserTokens.FirstOrDefaultAsync(t =>
            t.AuthProviderId == authProvider.Id &&
            t.TokenTypeId == tokenType.Id &&
            t.UserId == foundUser.Id
        );

        string accessToken = await tokenService.GenerateAccessToken(foundUser);
        string refreshToken = tokenService.GenerateRefreshToken(out DateTime issuedOn, out DateTime expiresOn);

        var userToken = tokenInDatabase ?? new UserToken()
        {
            AuthProviderId = authProvider.Id,
            TokenTypeId = tokenType.Id,
            UserId = foundUser.Id
        };

        userToken.Value = refreshToken;
        userToken.IssuedOnDateTime = issuedOn;
        userToken.ExpiresOnDateTIme = expiresOn;

        database.UserTokens.Update(userToken);
        await database.SaveChangesAsync();
        
        return Result.Success(new LoginResponse(accessToken, refreshToken));
    }
}
