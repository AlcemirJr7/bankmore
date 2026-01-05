using Asp.Versioning;
using Core.Infrastructure.Abstractions;
using Core.Infrastructure.Extensions;
using Core.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transferencia.Application.Features.TransferirInterno;

namespace Transferencia.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TransferenciaController(IMediator mediator) : AbstractController
{
    [HttpPost("TransferirInterno")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IResult> TransferirInterno(
        TransferirInternoRequest request,
        CancellationToken ct)
    {
        request.IdContaLogada = GetUserId();
        request.ChaveIdempotencia = GetIdempotenciaKey();

        var result = await mediator.Send(request, ct);

        return result.Response();
    }
}
