using KafkaFlow;
using Microsoft.AspNetCore.Builder;

namespace Core.Infrastructure.Extensions;

public static class KafkaExtensions
{
    public static async Task<WebApplication> UseKafkaAsync(this WebApplication app)
    {
        var kafkaBus = app.Services.CreateKafkaBus();
        await kafkaBus.StartAsync();

        app.Lifetime.ApplicationStopping.Register(async () => await kafkaBus.StopAsync());

        return app;
    }
}
