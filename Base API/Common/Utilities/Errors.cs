using FluentValidation.Results;

namespace BaseAPI.Common.Utilities;

# region Do not touch
public record Error
{
    public string? Message { get; set; }

    public Error WithMessage(string? message)
    {
        Message = message;
        return this;
    }
}

public record ValidationError : Error
{
    public ValidationResult ValidationResult { get; private set; } = null!;

    public ValidationError WithResult(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
        return this;
    }
}
#endregion

public record InternalServerError : Error;
public record UnauthorizedError : Error;
public record ConflictError : Error;
public record NotFoundError : Error;
