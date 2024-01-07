using BaseAPI.Common.Constants;
using BaseAPI.Common.Utilities;
using BaseAPI.Database;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace BaseAPI.Features.Profile.UploadAvatar;

public interface IUploadAvatarService
{
    Task<Result> UploadAsync(long userId, UploadAvatarRequest request);
}

public class UploadAvatarService
(
    IValidator<UploadAvatarRequest> validator,
    DatabaseContext database,
    IWebHostEnvironment environment
): IUploadAvatarService
{
    public async Task<Result> UploadAsync(long userId, UploadAvatarRequest request)
    {
        var user = await database.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == userId);
        
        if (user is null)
            return Result.NotFoundError();
        if (user.Profile is null)
            return Result.NotFoundError("Profile not found for this user");

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.ValidationError(validationResult);

        var avatarFolder = Path.Join(environment.WebRootPath, Folders.Avatars);
        string avatarFileName;

        if (user.Profile.AvatarFileName is null)
        {
            var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var extension = Path.GetExtension(request.File.FileName);

            avatarFileName = $"{time}{extension}";
            user.Profile.AvatarFileName = avatarFileName;
        }
        else
        {
            avatarFileName = user.Profile.AvatarFileName;
        }

        using var file = File.OpenWrite(Path.Join(avatarFolder, avatarFileName));
        await request.File.CopyToAsync(file);

        await database.SaveChangesAsync();
        return Result.Success();
    }
}
