using Asp.Versioning;
using ContaCorrente.Application.Features.Commands.Cadastrar;
using ContaCorrente.Application.Features.Commands.Inativar;
using ContaCorrente.Application.Features.Commands.Movimentar;
using ContaCorrente.Application.Features.Queries.ConsultaId;
using ContaCorrente.Application.Features.Queries.ConsultaSaldo;
using Core.Infrastructure.Abstractions;
using Core.Infrastructure.Extensions;
using Core.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContaCorrente.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ContaCorrenteController(IMediator mediator) : AbstractController
    {

        [HttpPost("Cadastrar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<CadastrarResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IResult> Cadastrar(
            CadastrarRequest request,
            CancellationToken ct)
        {
            var result = await mediator.Send(request, ct);

            return result.Response();
        }

        [HttpPost("Inativar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IResult> Inativar(
            InativarRequest request,
            CancellationToken ct)
        {
            var result = await mediator.Send(request, ct);

            return result.Response();
        }

        [HttpPost("Movimentar")]
        [ProducesResponseType(typeof(ApiResponse<CriarMovimentoResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IResult> Movimentar(
            CriarMovimentoRequest request,
            CancellationToken ct)
        {
            request.IdContaLogada = GetUserId();

            var result = await mediator.Send(request, ct);

            return result.Response();
        }

        [HttpGet("ConsultaSaldo")]
        [ProducesResponseType(typeof(ApiResponse<ConsultaSaldoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IResult> ConsultaSaldo(CancellationToken ct)
        {
            var result = await mediator.Send(
                new ConsultaSaldoRequest
                {
                    IdContaCorrente = GetUserId()
                }, ct);

            return result.Response();
        }

        [HttpGet("ConsultaId")]
        [ProducesResponseType(typeof(ApiResponse<ConsultaIdResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IResult> ConsultaId([FromQuery] ConsultaIdRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(request, ct);

            return result.Response();
        }
    }
}
