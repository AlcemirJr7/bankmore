using Core.Infrastructure.Abstractions;
using Core.Infrastructure.Idempotencia;
using Core.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tarifa.Application.Features.Tarifar;

namespace Tarifa.Api.Controllers;

public class TarifaController(IMediator mediator) : AbstractApiController
{
    [HttpPost("Tarifar")]
    [SkipIdempotency]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Tarifar(
        TarifarRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);

        return Response(result);
    }
}
