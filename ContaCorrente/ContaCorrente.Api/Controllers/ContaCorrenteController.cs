using ContaCorrente.Application.Features.Commands.Cadastrar;
using ContaCorrente.Application.Features.Commands.Inativar;
using ContaCorrente.Application.Features.Commands.Movimentar;
using ContaCorrente.Application.Features.Queries.ConsultaId;
using ContaCorrente.Application.Features.Queries.ConsultaSaldo;
using Core.Infrastructure.Abstractions;
using Core.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContaCorrente.API.Controllers
{
    public class ContaCorrenteController(IMediator mediator) : AbstractApiController
    {

        [HttpPost("Cadastrar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<CadastrarResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Cadastrar(
            CadastrarRequest request,
            CancellationToken ct)
        {
            var result = await mediator.Send(request, ct);

            return Response(result);
        }

        [HttpPost("Inativar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Inativar(
            InativarRequest request,
            CancellationToken ct)
        {
            var result = await mediator.Send(request, ct);

            return Response(result);
        }

        [HttpPost("Movimentar")]
        [ProducesResponseType(typeof(ApiResponse<CriarMovimentoResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Movimentar(
            CriarMovimentoRequest request,
            CancellationToken ct)
        {
            request.IdContaLogada = IdContaLogada;
            request.ChaveIdempotencia = ChaveIdempotencia;

            var result = await mediator.Send(request, ct);

            return Response(result);
        }

        [HttpGet("ConsultaSaldo")]
        [ProducesResponseType(typeof(ApiResponse<ConsultaSaldoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsultaSaldo(CancellationToken ct)
        {
            var result = await mediator.Send(
                new ConsultaSaldoRequest
                {
                    IdContaCorrente = IdContaLogada
                }, ct);

            return Response(result);
        }

        [HttpGet("ConsultaId")]
        [ProducesResponseType(typeof(ApiResponse<ConsultaIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsultaId([FromQuery] ConsultaIdRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(request, ct);

            return Response(result);
        }
    }
}
