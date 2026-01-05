using Core.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Core.Infrastructure.Extensions;

public static class MiddlewaresExtensions
{
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionHandlingMiddleware>();
        builder.UseMiddleware<IdempotenciaMiddleware>();

        return builder;
    }
}
