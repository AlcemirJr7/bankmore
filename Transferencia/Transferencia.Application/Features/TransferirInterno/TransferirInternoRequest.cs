using Core.Abstractions;
using Core.Response;
using MediatR;

namespace Transferencia.Application.Features.TransferirInterno;

public sealed class TransferirInternoRequest : RequestBase, IRequest<ApiResponse>
{
    public int NumeroContaDestino { get; init; } = 0;
    public decimal Valor { get; init; } = 0;
}
