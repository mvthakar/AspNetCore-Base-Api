using Microsoft.AspNetCore.Mvc;

using BaseAPI.Common.Extensions;

namespace BaseAPI.Features.Auth.SignUp;

public static class SignUpHandler
{
    public static async Task<IResult> Handle(SignUpRequest request, [FromServices] ISignUpService signUpService)
    {
        var result = await signUpService.SignUpAsync(request);
        return result.AsHttpResponse();
    }
}
