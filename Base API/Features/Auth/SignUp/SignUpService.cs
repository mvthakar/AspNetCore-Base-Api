using BaseAPI.Common.Utilities;
using BaseAPI.Database;
using BaseAPI.Database.Models.Identity;

using FluentValidation;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static BaseAPI.Common.Constants.Auth;

namespace BaseAPI.Features.Auth.SignUp;

public interface ISignUpService
{
    Task<Result<SignUpResponse>> SignUpAsync(SignUpRequest request);
}

public class SignUpService(
    [FromServices] IValidator<SignUpRequest> validator, 
    [FromServices] UserManager<User> userManager, 
    [FromServices] DatabaseContext database
) : ISignUpService
{
    public async Task<Result<SignUpResponse>> SignUpAsync(SignUpRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result.ValidationError(validationResult);

        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
            return Result.ConflictError("Email is already in use");

        var user = request.AsUser();
        user.AuthProvider = await database.AuthProviders.FirstAsync(provider => provider.Name == "Email");

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var message = result.Errors.First().Description;
            return Result.InternalServerError(message);
        }

        await userManager.AddToRoleAsync(user, Roles.User);
        return Result.Success(new SignUpResponse());
    }
}
