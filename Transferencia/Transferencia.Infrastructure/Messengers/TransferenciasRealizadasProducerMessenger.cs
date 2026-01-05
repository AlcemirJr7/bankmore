using Core.Messengers;
using Core.Messengers.Models;
using KafkaFlow.Producers;
using Serilog;
using Transferencia.Application.Messengers;

namespace Transferencia.Infrastructure.Messengers;

public sealed class TransferenciasRealizadasProducerMessenger(IProducerAccessor producerAccessor)
    : ITransferenciasRealizadasProducerMessenger
{
    public async Task EnviarAsync(TransferenciasRealizadasMessage messege)
    {
        Log.Information($"Enviando mensagem de transferência realizada: {messege.IdTransferencia}");

        var producer = producerAccessor.GetProducer(KafkaConsts.TransferenciasProducer);

        await producer.ProduceAsync(
            topic: KafkaConsts.TransferenciasRealizadasTopico,
            messageKey: messege.IdTransferencia,
            messageValue: messege
        );

        Log.Information($"Mensagem de transferência realizada enviada: {messege.IdTransferencia}");
    }
}
