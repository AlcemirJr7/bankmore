using Core.Infrastructure.Extensions;
using Core.Messengers;
using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tarifa.Application.Messengers;
using Tarifa.Domain.Repositories;
using Tarifa.Infrastructure.Messengers.Consumers;
using Tarifa.Infrastructure.Messengers.Producers;
using Tarifa.Infrastructure.Repositories;

namespace Tarifa.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        services.AddSecurity(configuration);

        services.ConfigureApiBehavior();

        services.AddRepositories();

        services.AddTarifasMessengers(configuration);

        services.AddSwaggerConfiguration("Tarifa");

        services.AddMediatR(c => c.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.Load("Tarifa.Application")));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICommandRepository, CommandRepository>();

        return services;
    }

    private static IServiceCollection AddTarifasMessengers(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<TransferenciasRealizadasMessageHandler>();
        services.AddSingleton<ITarifasRealizadasProducerMessenger, TarifasRealizadasProducerMessenger>();

        services.AddKafka(kafka => kafka
                .UseConsoleLog()
                .AddCluster(cluster => cluster
                    .WithBrokers([configuration["Kafka:Brokers"] ?? "localhost:9092"])
                    .CreateTopicIfNotExists(KafkaConsts.TransferenciasRealizadasTopico, 5, 1)
                    .AddConsumer(consumer => consumer
                        .Topic(KafkaConsts.TransferenciasRealizadasTopico)
                        .WithGroupId("tarifas-consumer-group")
                        .WithName("transferencias-tarifas-consumer")
                        .WithBufferSize(1000)
                        .WithWorkersCount(5)
                        .AddMiddlewares(m => m
                            .AddDeserializer<JsonCoreDeserializer>()
                            .AddTypedHandlers(h => h.AddHandler<TransferenciasRealizadasMessageHandler>())
                        )
                    )
                    .CreateTopicIfNotExists(KafkaConsts.TarifasRealizadasTopico, 5, 1)
                    .AddProducer(KafkaConsts.TarifasProducer, producer => producer
                        .DefaultTopic(KafkaConsts.TarifasRealizadasTopico)
                        .AddMiddlewares(m => m
                            .AddSerializer<JsonCoreSerializer>()
                        )
                    )
                )
            );

        return services;
    }
}
