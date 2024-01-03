namespace BaseAPI.Features.Auth.SignUp;

public class SignUpRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
