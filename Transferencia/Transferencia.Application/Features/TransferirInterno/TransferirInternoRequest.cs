using Core.Abstractions;
using Core.DataAnnotations;
using Core.Response;
using MediatR;

namespace Transferencia.Application.Features.TransferirInterno;

public sealed class TransferirInternoRequest : RequestBase, IRequest<ApiResponse>
{
    public int NumeroContaDestino { get; init; } = 0;

    [ValorMaiorZero]
    public decimal Valor { get; init; } = 0;
}
