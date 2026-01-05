using ContaCorrente.Domain.Entities;

namespace ContaCorrente.Domain.Repositories;

public interface IContaCorrenteCommandRepository
{
    Task<bool> CadastrarAsync(ContaCorrenteEntity contaCorrente, CancellationToken ct = default);
    Task<bool> InativarAsync(string idContaCorrente, CancellationToken ct = default);
    Task<bool> MovimentarAsync(MovimentoEntity movimento, CancellationToken ct = default);
}
