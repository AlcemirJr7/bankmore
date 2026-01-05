namespace Core.Response;

public sealed record ErrorDetails(string Type, string Message, int StatusCode = ApiStatusCode.BadRequest);