using Core.Infrastructure.Database;
using Dapper;
using Tarifa.Domain.Entities;
using Tarifa.Domain.Repositories;

namespace Tarifa.Infrastructure.Repositories;

public class CommandRepository(IDbConnectionFactory connectionFactory) : ICommandRepository
{
    public async Task<bool> CreateAsync(TarifaEntity entity, CancellationToken ct)
    {
        const string sql = @"INSERT INTO tarifa
                                (idtarifa, idcontacorrente, datamovimento, valor)
                             VALUES(@IdTarifa, @IdContaCorrente, @DataMovimento, @Valor);";

        using var conn = connectionFactory.CreateConnection();

        var result = await conn.ExecuteAsync(sql, new
        {
            entity.IdTarifa,
            entity.IdContaCorrente,
            entity.DataMovimento,
            entity.Valor
        });

        return result > 0;
    }
}
