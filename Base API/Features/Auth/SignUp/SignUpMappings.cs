using BaseAPI.Database.Models.Identity;

namespace BaseAPI.Features.Auth.SignUp;

public static class SignUpMappings
{
    public static User AsUser(this SignUpRequest request)
    {
        // TODO: Set EmailConfirmed to false
        var user = new User() { UserName = request.Email, Email = request.Email, EmailConfirmed = true };
        return user;
    }
}
