namespace BaseAPI.Features.Profile.Get;

public static class GetProfileMappings
{
    public static GetProfileResponse AsResponse(this Database.Models.Profile profile)
    {
        return new(
            profile.FullName,
            profile.Address,
            profile.City,
            profile.State,
            profile.Pincode,
            profile.AvatarFileName
        );
    }
}
