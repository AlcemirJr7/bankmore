using Core.Response;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Text.Json;

namespace Core.Infrastructure.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Ocorreu uma exceção não tratada.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            InvalidOperationException => ApiStatusCode.BadRequest,
            UnauthorizedAccessException => ApiStatusCode.Unauthorized,
            HttpRequestException => ApiStatusCode.BadRequest,
            KeyNotFoundException => ApiStatusCode.NotFound,
            ArgumentNullException => ApiStatusCode.BadRequest,
            ArgumentException => ApiStatusCode.BadRequest,
            _ => ApiStatusCode.InternalServerError
        };

        var response = ApiResponse.Failure(new ErrorDetails(
            exception.GetType().Name,
            exception.Message,
            statusCode));

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}
