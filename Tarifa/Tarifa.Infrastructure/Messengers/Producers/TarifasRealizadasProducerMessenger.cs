using Core.Messengers;
using Core.Messengers.Models;
using KafkaFlow.Producers;
using Serilog;
using Tarifa.Application.Messengers;

namespace Tarifa.Infrastructure.Messengers.Producers;

public sealed class TarifasRealizadasProducerMessenger(IProducerAccessor producerAccessor)
    : ITarifasRealizadasProducerMessenger
{
    public async Task EnviarAsync(TarifasRealizadasMessage messege)
    {
        Log.Information($"Enviando mensagem de tarifa realizada: {messege.IdTarifa}");

        var producer = producerAccessor.GetProducer(KafkaConsts.TarifasProducer);

        await producer.ProduceAsync(
            topic: KafkaConsts.TarifasRealizadasTopico,
            messageKey: messege.IdTarifa,
            messageValue: messege
        );


        Log.Information($"Mensagem de tarifa realizada enviada: {messege.IdTarifa}");
    }
}
