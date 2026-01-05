using Core.Infrastructure.Extensions;
using Core.Infrastructure.Security.Auth;
using Core.Messengers;
using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Transferencia.Application.Features.TransferirInterno.Validation;
using Transferencia.Application.Gateways;
using Transferencia.Application.Messengers;
using Transferencia.Domain.Repositories;
using Transferencia.Infrastructure.Gateways;
using Transferencia.Infrastructure.Messengers;
using Transferencia.Infrastructure.Models;
using Transferencia.Infrastructure.Repositories;

namespace Transferencia.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        services.AddSecurity(configuration);

        services.ConfigureApiBehavior();

        services.AddRepositories();

        services.AddTransferenciasMessengers(configuration);

        services.AddGateways(configuration);

        services.AddValidators();

        services.AddSwaggerConfiguration("Transferencia");

        services.AddMediatR(c => c.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.Load("Transferencia.Application")));

        services.AddHostedService();

        return services;
    }

    public static IServiceCollection AddTransferenciasMessengers(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ITransferenciasRealizadasProducerMessenger, TransferenciasRealizadasProducerMessenger>();

        services.AddKafka(kafka => kafka
                .UseConsoleLog()
                .AddCluster(cluster => cluster
                    .WithBrokers([configuration["Kafka:Brokers"] ?? "localhost:9092"])
                    .CreateTopicIfNotExists(KafkaConsts.TransferenciasRealizadasTopico, 5, 1)
                    .AddProducer(KafkaConsts.TransferenciasProducer, producer => producer
                        .DefaultTopic(KafkaConsts.TransferenciasRealizadasTopico)
                        .AddMiddlewares(m => m
                            .AddSerializer<JsonCoreSerializer>()
                    )
                )
            ));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITransferenciaCommandRepository, TransferenciaCommandRepository>();

        return services;
    }

    private static IServiceCollection AddGateways(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiSettings>(configuration.GetSection("ContaCorrenteApiSettings"));

        services.AddHttpClient<IContaCorrenteApiGateway, ContaCorrenteApiGateway>((serviceProvider, client) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;

            client.BaseAddress = new Uri(settings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(settings.Timeout);
        })
        .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<ITransferirInternoValidator, TransferirInternoValidator>();
        return services;
    }
}
