namespace BaseAPI.Features.Profile.Get;

public record GetProfileResponse(
    string FullName,
    string Address,
    string City,
    string State,
    string Pincode,
    string? Avatar
);
