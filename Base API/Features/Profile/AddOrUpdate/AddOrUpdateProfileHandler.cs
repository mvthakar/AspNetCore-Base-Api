using System.Security.Claims;

using BaseAPI.Common.Extensions;

using Microsoft.AspNetCore.Mvc;

namespace BaseAPI.Features.Profile.Update;

public static class AddOrUpdateProfileHandler
{
    public static async Task<IResult> Handle(AddOrUpdateProfileRequest request, ClaimsPrincipal user, [FromServices] IAddOrUpdateProfileService updateProfileService)
    {
        var result = await updateProfileService.AddOrUpdateProfileAsync(user.GetUserId(), request);
        return result.AsHttpResponse();
    }
}
