using Core.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Core.Infrastructure.Extensions;

public static class ApiBehaviorExtensions
{
    public static IServiceCollection ConfigureApiBehavior(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .SelectMany(x => x.Value!.Errors)
                    .Select(e =>
                    {
                        try
                        {
                            return JsonSerializer.Deserialize<ErrorDetails>(e.ErrorMessage);
                        }
                        catch
                        {
                            return new ErrorDetails(
                                "VALIDATION_ERROR",
                                e.ErrorMessage);
                        }
                    });

                return new BadRequestObjectResult(ApiResponse.Failure(errors.FirstOrDefault()));
            };
        });

        return services;
    }
}
