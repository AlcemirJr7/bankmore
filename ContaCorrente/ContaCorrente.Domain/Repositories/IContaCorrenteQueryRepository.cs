using ContaCorrente.Domain.Models.Inputs;
using ContaCorrente.Domain.Models.Results;

namespace ContaCorrente.Domain.Repositories;

public interface IContaCorrenteQueryRepository
{
    Task<int> BuscaNovoNumeroContaAsync(CancellationToken ct = default);
    Task<ContaCorrenteResultModel?> BuscaContaCorrenteAsync(BuscaGenericaInputModel input, CancellationToken ct = default);
    Task<ContaCorrenteResultModel?> BuscaPeloDocumentoAsync(string documento, CancellationToken ct = default);
    Task<ContaCorrenteResultModel?> BuscaPeloNumeroAsync(int numero, CancellationToken ct = default);
    Task<ContaCorrenteResultModel?> BuscaPeloIdAsync(string idContaCorrente, CancellationToken ct = default);
    Task<CredenciaisResultModel> BuscaCredenciaisPeloIdAsync(string idContaCorrente, CancellationToken ct = default);
    Task<ConsultaSaldoResultModel?> ConsultaSaldoAsync(string idContaCorrente, CancellationToken ct = default);
    Task<ConsultaIdResultModel?> ConsultaIdAsync(BuscaGenericaInputModel input, CancellationToken ct = default);
}
