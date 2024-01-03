using FluentValidation;

namespace BaseAPI.Features.Profile.Update;

public class AddOrUpdateProfileRequestValidator : AbstractValidator<AddOrUpdateProfileRequest>
{
    public AddOrUpdateProfileRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty();

        RuleFor(x => x.Address)
            .NotEmpty();

        RuleFor(x => x.City)
            .NotEmpty();

        RuleFor(x => x.State)
            .NotEmpty();

        RuleFor(x => x.Pincode)
            .NotEmpty();
    }
}
