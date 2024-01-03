using BaseAPI.Common.Constants;
using BaseAPI.Common.Utilities;

namespace BaseAPI.Common.Extensions;

public static class ResultExtensions
{
    public static IResult AsHttpResponse(this Result result)
    {
        if (result.IsSuccess)
            return Results.Ok();

        return result.Error.GetHttpError();
    }

    public static IResult AsHttpResponse<T>(this Result<T> result) where T : class
    {
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return result.Error.GetHttpError();
    }

    private static IResult GetHttpError(this Error? error)
    {
        return error switch
        {
            ValidationError err => Results.ValidationProblem(err.ValidationResult.ToDictionary()),
            UnauthorizedError err => Results.Problem(statusCode: StatusCodes.Status401Unauthorized, detail: err.Message),
            ConflictError err => Results.Conflict(err.Message),
            NotFoundError err => Results.NotFound(err.Message),

            InternalServerError err => Results.Problem(err.Message),
            Error err => Results.Problem(err.Message),
            _ => Results.Problem()
        };
    }
}
