using Transferencia.Domain.Entities;

namespace Transferencia.Domain.Repositories;

public interface ITransferenciaCommandRepository
{
    Task<bool> CriarAsync(TransferenciaEntity entity, CancellationToken ct);
    Task<bool> AlteraStatusAsync(TransferenciaEntity entity, CancellationToken ct);
}
