using ContaCorrente.Application.Security.Login;
using Core.Infrastructure.Abstractions;
using Core.Infrastructure.Idempotencia;
using Core.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContaCorrente.Api.Controllers
{
    public class LoginController(IMediator mediator) : AbstractApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [SkipIdempotency]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logar(
            LoginRequest request,
            CancellationToken ct)
        {
            var result = await mediator.Send(request, ct);

            return Response(result);
        }
    }
}
