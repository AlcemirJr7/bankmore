using Core.Infrastructure.Abstractions;
using Core.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transferencia.Application.Features.TransferirInterno;

namespace Transferencia.Api.Controllers;

public class TransferenciaController(IMediator mediator) : AbstractApiController
{
    [HttpPost("TransferirInterno")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TransferirInterno(
        TransferirInternoRequest request,
        CancellationToken ct)
    {
        request.IdContaLogada = IdContaLogada;
        request.ChaveIdempotencia = ChaveIdempotencia;

        var result = await mediator.Send(request, ct);

        return Response(result);
    }
}
