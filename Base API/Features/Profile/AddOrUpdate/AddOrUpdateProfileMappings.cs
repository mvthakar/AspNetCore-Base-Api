namespace BaseAPI.Features.Profile.Update;

public static class AddOrUpdateProfileMappings
{
    public static AddOrUpdateProfileResponse AsResponse(this Database.Models.Profile profile)
    {
        return new(
            profile.FullName,
            profile.Address,
            profile.City,
            profile.State,
            profile.Pincode
        );
    }

    public static void UpdateFrom(this Database.Models.Profile profile, long userId, AddOrUpdateProfileRequest request)
    {
        profile.FullName = request.FullName;
        profile.Address = request.Address;
        profile.City = request.City;
        profile.State = request.State;
        profile.Pincode = request.Pincode;
        profile.UserId = userId;
    }
}
