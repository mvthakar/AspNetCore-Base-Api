using BaseAPI.Common.Constants;

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

    public static Result Fail() => new();
    public static Result Fail(Error error) => new() { Error = error };
}
