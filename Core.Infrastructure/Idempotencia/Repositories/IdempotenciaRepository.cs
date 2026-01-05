using Core.Idempotencia;
using Core.Infrastructure.Database;
using Dapper;

namespace Core.Infrastructure.Idempotencia.Repositories;

public class IdempotenciaRepository(IDbConnectionFactory connectionFactory) : IIdempotenciaRepository
{
    public async Task<bool> CreateAsync(IdempotenciaEntity entity)
    {
        const string sql = @"INSERT INTO idempotencia
                                (chave_idempotencia, requisicao, resultado, statuscode, datacriacao)
                            VALUES(@ChaveIdempotencia, @Requisicao, @Resultado, @StatusCode, @DataCriacao);";

        using var conn = connectionFactory.CreateConnection();

        var result = await conn.ExecuteAsync(sql, new
        {
            entity.ChaveIdempotencia,
            entity.Requisicao,
            entity.Resultado,
            entity.StatusCode,
            entity.DataCriacao
        });

        return result > 0;
    }

    public async Task<IdempotenciaEntity?> GetAsync(string chaveIdempotencia)
    {
        const string sql = @"SELECT chave_idempotencia as ChaveIdempotencia, 
                                    requisicao, 
                                    resultado,
                                    statuscode
                               FROM idempotencia
                            where chave_idempotencia = @ChaveIdempotencia";

        using var conn = connectionFactory.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<IdempotenciaEntity>(sql, new { ChaveIdempotencia = chaveIdempotencia });
    }

    public async Task<bool> CleanUpAsync(int tempo)
    {
        const string sql = @"DELETE FROM idempotencia
                             WHERE datacriacao <= @Data";

        using var conn = connectionFactory.CreateConnection();

        var result = await conn.ExecuteAsync(sql, new
        {
            Data = DateTime.Now.AddMinutes(Math.Abs(tempo) * -1)
        });

        return result > 0;
    }
}
