using Core.Response;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Core.Infrastructure.Extensions;

public static class ApiResponseExtensions
{
    public static IResult Response<T>(this ApiResponse<T> result, string? uri = null)
    {
        try
        {
            return result.StatusCode switch
            {
                StatusCodes.Status200OK => Results.Ok(result),
                StatusCodes.Status201Created => Results.Created(uri, result),
                StatusCodes.Status204NoContent => Results.NoContent(),
                StatusCodes.Status400BadRequest => Results.BadRequest(result),
                StatusCodes.Status401Unauthorized => Results.Unauthorized(),
                StatusCodes.Status403Forbidden => Results.StatusCode((int)HttpStatusCode.Forbidden),
                StatusCodes.Status404NotFound => Results.NotFound(result),
                _ => Results.StatusCode((int)HttpStatusCode.InternalServerError)
            };
        }
        catch (Exception)
        {
            return Results.BadRequest(result);
        }
    }

    public static IResult Response(this ApiResponse result, string? uri = null)
    {
        try
        {
            return result.StatusCode switch
            {
                StatusCodes.Status200OK => Results.Ok(result),
                StatusCodes.Status201Created => Results.Created(uri, result),
                StatusCodes.Status204NoContent => Results.NoContent(),
                StatusCodes.Status400BadRequest => Results.BadRequest(result),
                StatusCodes.Status401Unauthorized => Results.Unauthorized(),
                StatusCodes.Status403Forbidden => Results.StatusCode((int)HttpStatusCode.Forbidden),
                StatusCodes.Status404NotFound => Results.NotFound(result),
                _ => Results.StatusCode((int)HttpStatusCode.InternalServerError)
            };
        }
        catch (Exception)
        {
            return Results.BadRequest(result);
        }
    }
}
