using BaseAPI.Common.Constants;
using BaseAPI.Common.Utilities;
using BaseAPI.Database;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using static BaseAPI.Common.Constants.Auth;

namespace BaseAPI.Features.Auth.Token.Refresh;

public interface IRefreshTokenService
{
    Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, long userId);
}

public class RefreshTokenService(
    DatabaseContext database,
    IValidator<RefreshTokenRequest> validator,
    ITokenService tokenService
) : IRefreshTokenService
{
    public async Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, long userId)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Fail(Errors.Validation.WithResult(validationResult));

        var user = await database.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            return Result.Fail(Errors.NotFound);

        var tokenInDatabase = await database.UserTokens.Include(t => t.AuthProvider).Include(t => t.TokenType).FirstOrDefaultAsync(t =>
            t.AuthProvider.Name == Providers.Email &&
            t.TokenType.Name == TokenTypes.RefreshToken &&
            t.Value == request.Token &&
            t.UserId == userId
        );

        if (tokenInDatabase is null)
            return Result.Fail(Errors.Unauthorized);

        var accessToken = await tokenService.GenerateAccessToken(user);
        var refreshToken = tokenService.GenerateRefreshToken(out DateTime issuedOn, out DateTime expiresOn);

        tokenInDatabase.Value = refreshToken;
        tokenInDatabase.IssuedOnDateTime = issuedOn;
        tokenInDatabase.ExpiresOnDateTIme = expiresOn;

        await database.SaveChangesAsync();
        return Result.Success(new RefreshTokenResponse(accessToken, refreshToken));
    }
}
