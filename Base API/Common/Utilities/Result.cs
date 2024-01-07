using FluentValidation.Results;

namespace BaseAPI.Common.Utilities;

public class ResultBase
{
    public bool IsSuccess { get; set; }
    public Error? Error { get; set; } = null!;
}

public class Result<T> : ResultBase where T : class
{
    public T Value { get; set; } = null!;

    public static implicit operator Result<T>(Result result) => new()
    {
        IsSuccess = result.IsSuccess,
        Error = result.Error
    };
}

public class Result : ResultBase
{
    public static Result Success() => new() { IsSuccess = true };
    public static Result<T> Success<T>(T value) where T : class => new() { IsSuccess = true, Value = value };

    private static Result Fail(Error error) => new() { Error = error };

    public static Result ValidationError(ValidationResult validationResult) => Fail(new ValidationError().WithResult(validationResult));
    public static Result InternalServerError(string? message = null) => Fail(new InternalServerError().WithMessage(message));
    public static Result UnauthorizedError(string? message = null) => Fail(new UnauthorizedError().WithMessage(message));
    public static Result ConflictError(string? message = null) => Fail(new ConflictError().WithMessage(message));
    public static Result NotFoundError(string? message = null) => Fail(new NotFoundError().WithMessage(message));
}
