using Core.Idempotencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Infrastructure.Abstractions;

[ApiController]
[Produces("application/json")]
[Authorize]
public abstract class AbstractController : ControllerBase
{
    protected string GetUserId()
    {
        return User.FindFirst("idContaCorrente")?.Value ?? string.Empty;
    }

    protected string GetIdempotenciaKey()
    {
        HttpContext.Request.Headers.TryGetValue(IdempotenciaConsts.HeaderKeyName, out var chaveIdempotencia);

        return chaveIdempotencia.ToString();
    }
}