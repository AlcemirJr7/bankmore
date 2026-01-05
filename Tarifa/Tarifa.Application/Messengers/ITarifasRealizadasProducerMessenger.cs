using Core.Abstractions;
using Core.Messengers.Models;

namespace Tarifa.Application.Messengers;

public interface ITarifasRealizadasProducerMessenger : IMessenger<TarifasRealizadasMessage>
{
}