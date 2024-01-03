using BaseAPI.Common.Constants;
using BaseAPI.Common.Utilities;
using BaseAPI.Database;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace BaseAPI.Features.Auth.Logout;

public interface ILogoutService
{
    Task<Result> LogoutAsync(LogoutRequest request, long userId);
    Task<Result> LogoutAllAsync(long userId);
}

public class LogoutService(
    DatabaseContext database,
    IValidator<LogoutRequest> validator
) : ILogoutService
{
    public async Task<Result> LogoutAsync(LogoutRequest request, long userId)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.Fail(Errors.Validation.WithResult(validationResult));

        var userToken = await database.UserTokens.FirstOrDefaultAsync(ut => ut.UserId == userId);
        if (userToken is null)
            return Result.Fail(Errors.Unauthorized);

        database.UserTokens.Remove(userToken);
        await database.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> LogoutAllAsync(long userId)
    {
        var numberOfDeletedTokens = await database.UserTokens.Where(token => token.UserId == userId).ExecuteDeleteAsync();
        return numberOfDeletedTokens > 0 ? Result.Success() : Result.Fail(Errors.Unauthorized);
    }
}
