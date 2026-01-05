using Core.Abstractions;
using Core.Messengers.Models;

namespace Transferencia.Application.Messengers;

public interface ITransferenciasRealizadasProducerMessenger : IMessenger<TransferenciasRealizadasMessage>
{
}
