namespace BaseAPI.Features.Auth.Logout;

public class LogoutRequest
{
    public string RefreshToken { get; set; } = null!;
}
