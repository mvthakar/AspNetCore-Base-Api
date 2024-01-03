namespace BaseAPI.Features.Profile.UploadAvatar;

public class UploadAvatarRequest
{
    public IFormFile File { get; set; } = null!;
}
