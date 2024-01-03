using FluentValidation;

namespace BaseAPI.Features.Auth.SignUp;

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8).WithMessage("Password must be minimum 8 characters long")
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one digit")
            .Matches(@"[^A-Za-z0-9]+").WithMessage("Password must contain at least one special character.");
    }
}
