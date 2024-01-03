using FluentValidation;

namespace BaseAPI.Features.Auth.Token.Refresh;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}
