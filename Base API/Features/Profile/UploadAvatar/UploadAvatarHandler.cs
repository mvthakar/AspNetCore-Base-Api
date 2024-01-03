using System.Security.Claims;

using BaseAPI.Common.Extensions;

using Microsoft.AspNetCore.Mvc;

namespace BaseAPI.Features.Profile.UploadAvatar;

public static class UploadAvatarHandler
{
    public static async Task<IResult> Handle(
        IFormFile file,
        ClaimsPrincipal user,
        [FromServices] IUploadAvatarService uploadAvatarService
    )
    {
        var result = await uploadAvatarService.UploadAsync(user.GetUserId(), new UploadAvatarRequest() { File = file });
        return result.AsHttpResponse();
    }
}
