using System.Security.Claims;

using BaseAPI.Common.Extensions;

using Microsoft.AspNetCore.Mvc;

namespace BaseAPI.Features.Profile.Get;

public static class GetProfileHandler
{
    public static async Task<IResult> Handle(ClaimsPrincipal user, [FromServices] IGetProfileService getProfileService)
    {
        var result = await getProfileService.GetProfileAsync(user.GetUserId());
        return result.AsHttpResponse();
    }
}
