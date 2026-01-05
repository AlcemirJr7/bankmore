using Tarifa.Domain.Entities;

namespace Tarifa.Domain.Repositories;

public interface ICommandRepository
{
    Task<bool> CreateAsync(TarifaEntity entity, CancellationToken ct);
}
