using FluentValidation;

namespace BaseAPI.Features.Profile.UploadAvatar;

public class UploadAvatarRequestValidator : AbstractValidator<UploadAvatarRequest>
{
    private static readonly string[] allowedFileTypes = ["image/jpeg", "image/jpg", "image/png"];

    public UploadAvatarRequestValidator()
    {
        RuleFor(x => x.File.Length)
            .Must(size => size > 0 && size < 1048576)
            .WithMessage("Avatar size must be 1 MB or less");

        RuleFor(x => x.File.ContentType)
            .Must(type => allowedFileTypes.Contains(type))
            .WithMessage("Only JPG or PNG files are allowed");
    }
}
