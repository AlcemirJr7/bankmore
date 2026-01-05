using Core.Extensions;

namespace Core.Response;

public class ApiResponse
{
    public bool IsSuccess { get; init; }
    public int StatusCode { get; init; }
    public ErrorDetails? Error { get; init; }

    public string Timestamp { get; init; } = DateTime.Now.BrStr();

    public ApiResponse(bool isSuccess, int statusCode, ErrorDetails? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
    }

    public static ApiResponse Success() =>
        new(true, ApiStatusCode.Ok);

    public static ApiResponse SuccessNoContent() =>
        new(true, ApiStatusCode.NoContent);

    public static ApiResponse<T> Success<T>(T data) =>
        new(data, true, ApiStatusCode.Ok);

    public static ApiResponse<T> SuccessCreated<T>(T data) =>
        new(data, true, ApiStatusCode.Created);

    public static ApiResponse Failure(ErrorDetails? error) =>
        new(false, error!.StatusCode, error);

    public static ApiResponse<T> Failure<T>(ErrorDetails? error) =>
        new(default, false, error!.StatusCode, error);
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; init; }

    public ApiResponse(T? data, bool isSuccess, int statusCode, ErrorDetails? error = null)
        : base(isSuccess, statusCode, error)
    {
        Data = data;
    }

    public static implicit operator ApiResponse<T>(T? data) =>
        data is not null ? Success(data) : Failure<T>(default);
}