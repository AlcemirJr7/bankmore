using Core.Response;

namespace ContaCorrente.Application.Features.Commands.Movimentar.Service;

public interface ICriarMovimentoService
{
    Task<ApiResponse<CriarMovimentoResponse>> CriaMovimentoAsync(CriarMovimentoRequest request, CancellationToken ct);
}
