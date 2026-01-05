using Asp.Versioning;
using Core.Infrastructure.Abstractions;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Idempotencia;
using Core.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tarifa.Application.Features.Tarifar;

namespace Tarifa.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TarifaController(IMediator mediator) : AbstractController
{
    [HttpPost("Tarifar")]
    [SkipIdempotency]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IResult> Tarifar(
        TarifarRequest request,
        CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);

        return result.Response();
    }
}
