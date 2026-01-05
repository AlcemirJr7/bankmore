using Core.Infrastructure.Idempotencia.Repositories;
using Microsoft.Extensions.Hosting;

namespace Core.Infrastructure.HostedServices;

public sealed class IdempotenciaCleanupHostedService(
    IIdempotenciaRepository idempotenciaRepository) : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(2);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_interval, stoppingToken);
            await idempotenciaRepository.CleanUpAsync(_interval.Minutes);
        }
    }
}
