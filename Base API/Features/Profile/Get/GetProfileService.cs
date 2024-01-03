using BaseAPI.Common.Constants;
using BaseAPI.Common.Utilities;
using BaseAPI.Database;

using Microsoft.EntityFrameworkCore;

namespace BaseAPI.Features.Profile.Get;

public interface IGetProfileService
{
    Task<Result<GetProfileResponse>> GetProfileAsync(long userId);
}

public class GetProfileService(DatabaseContext database) : IGetProfileService
{
    public async Task<Result<GetProfileResponse>> GetProfileAsync(long userId)
    {
        var profile = await database.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile is null)
            return Result.Fail(Errors.NotFound);

        return Result.Success(profile.AsResponse());
    }
}
