using BaseAPI.Common.Utilities;
using BaseAPI.Database;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaseAPI.Features.Profile.Update;

public interface IAddOrUpdateProfileService
{
    Task<Result<AddOrUpdateProfileResponse>> AddOrUpdateProfileAsync(long userId, AddOrUpdateProfileRequest request);
}

public class AddOrUpdateProfileService(
    [FromServices] IValidator<AddOrUpdateProfileRequest> validator,
    [FromServices] DatabaseContext database
) : IAddOrUpdateProfileService
{
    public async Task<Result<AddOrUpdateProfileResponse>> AddOrUpdateProfileAsync(long userId, AddOrUpdateProfileRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.ValidationError(validationResult);

        var profile = await database.Profiles.FirstOrDefaultAsync(p => p.UserId == userId) ?? new Database.Models.Profile();
        profile.UpdateFrom(userId, request);

        database.Profiles.Update(profile);
        await database.SaveChangesAsync();

        return Result.Success(profile.AsResponse());
    }
}
