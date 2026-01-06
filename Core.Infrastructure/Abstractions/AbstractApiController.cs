using Asp.Versioning;
using Core.Response;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Core.Infrastructure.Abstractions;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class AbstractApiController : ControllerBase
{
    // Propriedades para IdContaLogada e ChaveIdempotencia (já existentes)
    protected string IdContaLogada => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.ToString() ?? string.Empty;
    protected string ChaveIdempotencia => Request.Headers["Idempotencia-Key"].ToString();

    // Método de extensão Response (já existente)
    // A implementação deve garantir que o StatusCode do ApiResponse seja usado para o StatusCode HTTP.
    protected IActionResult Response<TData>(ApiResponse<TData> apiResponse)
    {
        if (apiResponse.IsSuccess)
        {
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }
        else
        {
            // Para falhas, usamos o StatusCode fornecido no ApiResponse
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }
    }
}