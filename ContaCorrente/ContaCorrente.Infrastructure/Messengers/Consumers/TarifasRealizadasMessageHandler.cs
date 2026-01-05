using ContaCorrente.Application.Features.Commands.Movimentar;
using ContaCorrente.Application.Features.Commands.Movimentar.Service;
using Core.Messengers.Models;
using Core.ValueObjects;
using KafkaFlow;
using Serilog;

namespace ContaCorrente.Infrastructure.Messengers.Consumers;

public sealed class TarifasRealizadasMessageHandler : IMessageHandler<TarifasRealizadasMessage>
{
    public async Task Handle(IMessageContext context, TarifasRealizadasMessage message)
    {
        Log.Information(@$"Recebida mensagem de tarifa realizada para a conta 
                        {message.IdContaCorrente} no valor de {message.Valor}.");

        var service = context.DependencyResolver.Resolve<ICriarMovimentoService>();

        await service.CriaMovimentoAsync(
            new CriarMovimentoRequest
            {
                IdContaLogada = message.IdContaCorrente,
                Tipo = TipoMovimento.Debito,
                Valor = message.Valor
            }, CancellationToken.None);

        Log.Information($@"Movimento de tarifa realizado para a conta {message.IdContaCorrente} no valor de {message.Valor}.");
    }
}
