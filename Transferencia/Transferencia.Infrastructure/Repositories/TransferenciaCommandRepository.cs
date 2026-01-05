using Core.Infrastructure.Database;
using Dapper;
using Transferencia.Domain.Entities;
using Transferencia.Domain.Repositories;

namespace Transferencia.Infrastructure.Repositories;

public class TransferenciaCommandRepository(IDbConnectionFactory connectionFactory) : ITransferenciaCommandRepository
{
    public async Task<bool> AlteraStatusAsync(TransferenciaEntity entity, CancellationToken ct)
    {
        const string sql = @"UPDATE transferencia
                                SET status = @Status
                             WHERE idtransferencia= @IdTransferencia;";

        using var conn = connectionFactory.CreateConnection();

        var result = await conn.ExecuteAsync(sql, new
        {
            entity.IdTransferencia,
            entity.Status
        });

        return result > 0;
    }

    public async Task<bool> CriarAsync(TransferenciaEntity entity, CancellationToken ct)
    {
        const string sql = @"INSERT INTO transferencia 
                                (idtransferencia, idcontacorrente_origem, idcontacorrente_destino, datamovimento, valor)
                             VALUES(@IdTransferencia, @IdContaCorrenteOrigem, @IdContaCorrenteDestino, @DataMovimento, @Valor);";

        using var conn = connectionFactory.CreateConnection();

        var result = await conn.ExecuteAsync(sql, new
        {
            entity.IdTransferencia,
            entity.IdContaCorrenteOrigem,
            entity.IdContaCorrenteDestino,
            entity.DataMovimento,
            entity.Valor
        });

        return result > 0;
    }
}
