using Core.Extensions;
using Core.Messengers.Models;
using KafkaFlow;
using Microsoft.Extensions.Configuration;
using Serilog;
using Tarifa.Application.Messengers;
using Tarifa.Domain.Entities;
using Tarifa.Domain.Repositories;

namespace Tarifa.Infrastructure.Messengers.Consumers;

public sealed class TransferenciasRealizadasMessageHandler(
    ITarifasRealizadasProducerMessenger tarifasRealizadasProducerMessenger) : IMessageHandler<TransferenciasRealizadasMessage>
{
    public async Task Handle(
        IMessageContext context,
        TransferenciasRealizadasMessage message)
    {
        try
        {
            Log.Information($"Transferencia [{message.IdTransferencia}] recebida!");

            var commandRepository = context.DependencyResolver.Resolve<ICommandRepository>();
            var config = context.DependencyResolver.Resolve<IConfiguration>();

            var valorTarifa = config.GetValue<int?>("Tarifas:Transferencia") ?? 2;

            await commandRepository.CreateAsync(
                new TarifaEntity(
                    idContaCorrente: message.IdContaCorrente,
                    dataMovimento: DateTime.Now.BrStr(),
                    valor: valorTarifa),
                CancellationToken.None);

            await tarifasRealizadasProducerMessenger.EnviarAsync(
                new TarifasRealizadasMessage
                {
                    IdTarifa = Guid.NewGuid().ToString(),
                    IdContaCorrente = message.IdContaCorrente,
                    Valor = valorTarifa
                });

            Log.Information($"Transferencia [{message.IdTransferencia}] processada!");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Erro ao enviar menssagem de tarifa realizadas. {message}");
        }
    }
}
