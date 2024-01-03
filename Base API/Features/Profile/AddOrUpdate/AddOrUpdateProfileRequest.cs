namespace BaseAPI.Features.Profile.Update;

public record AddOrUpdateProfileRequest(
    string FullName,
    string Address,
    string City,
    string State,
    string Pincode
);
