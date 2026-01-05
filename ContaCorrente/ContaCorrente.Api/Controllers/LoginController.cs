using Asp.Versioning;
using ContaCorrente.Application.Security.Login;
using Core.Infrastructure.Abstractions;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Idempotencia;
using Core.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContaCorrente.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController(IMediator mediator) : AbstractController
    {
        [HttpPost]
        [AllowAnonymous]
        [SkipIdempotency]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IResult> Logar(
            LoginRequest request,
            CancellationToken ct)
        {
            var result = await mediator.Send(request, ct);

            return result.Response();
        }
    }
}
