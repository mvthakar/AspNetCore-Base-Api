using FluentValidation;

namespace BaseAPI.Features.Auth.Logout;

public class LogoutRequestValidator : AbstractValidator<LogoutRequest>
{
    public LogoutRequestValidator()
    {
        
    }
}
