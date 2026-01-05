using Core.Response;
using Transferencia.Application.Gateways.Models.Inputs;
using Transferencia.Application.Gateways.Models.Results;

namespace Transferencia.Application.Gateways;

public interface IContaCorrenteApiGateway
{
    Task<ApiResponse<CriaMovimentoResultModel>> CriaMovimentoAsync(
        CriaMovimentoInputModel input, string chaveIdempotencia, CancellationToken ct = default);
    Task<ApiResponse<SaldoContaResultModel>> ConsultaSaldoAsync(string idContaCorrente, CancellationToken ct = default);
    Task<ApiResponse<IdContaResultModel>> ConsultaIdAsync(string? idContaCorrente = null, int? numeroConta = null, CancellationToken ct = default);
}
