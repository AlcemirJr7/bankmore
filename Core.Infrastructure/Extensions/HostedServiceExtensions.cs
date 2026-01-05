using Core.Infrastructure.HostedServices;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Extensions;

public static class HostedServiceExtensions
{
    public static IServiceCollection AddHostedService(this IServiceCollection services)
    {
        services.AddHostedService<IdempotenciaCleanupHostedService>();

        return services;
    }
}
