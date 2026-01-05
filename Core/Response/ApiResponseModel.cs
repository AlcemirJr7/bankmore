using Core.Extensions;

namespace Core.Response;

public class ApiResponseModel
{
    public object? Data { get; set; }
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public ErrorDetails? Error { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now.Br();
}
