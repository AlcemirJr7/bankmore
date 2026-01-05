using ContaCorrente.Domain.Entities;
using ContaCorrente.Domain.Repositories;
using ContaCorrente.Domain.ValueObjects;
using Core.Infrastructure.Database;
using Dapper;

namespace ContaCorrente.Infrastructure.Repositories;

public class ContaCorrenteCommandsRepository(IDbConnectionFactory connectionFactory) : IContaCorrenteCommandRepository
{
    public async Task<bool> CadastrarAsync(ContaCorrenteEntity contaCorrente, CancellationToken ct = default)
    {
        const string sql = @"INSERT INTO contacorrente (idcontacorrente, numero, documento, nome, ativo, senha, salt) 
                             VALUES (@IdContaCorrente, @Numero, @Documento, @Nome, @Ativo, @Senha, @Salt);";

        using var conn = connectionFactory.CreateConnection();

        var result = await conn.ExecuteAsync(sql, new
        {
            contaCorrente.IdContaCorrente,
            contaCorrente.Numero,
            contaCorrente.Documento,
            contaCorrente.Nome,
            contaCorrente.Ativo,
            contaCorrente.Senha,
            contaCorrente.Salt
        });

        return result > 0;
    }

    public async Task<bool> InativarAsync(string idContaCorrente, CancellationToken ct = default)
    {
        const string sql = @"UPDATE contacorrente
                                SET ativo = @Ativo
                              WHERE idcontacorrente = @IdContaCorrente";

        using var conn = connectionFactory.CreateConnection();

        var result = await conn.ExecuteAsync(sql, new
        {
            idContaCorrente,
            FlagAtivo.Inativo
        });

        return result > 0;
    }

    public async Task<bool> MovimentarAsync(MovimentoEntity movimento, CancellationToken ct = default)
    {
        const string sql = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                             VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);";

        using var conn = connectionFactory.CreateConnection();

        var result = await conn.ExecuteAsync(sql, new
        {
            movimento.IdMovimento,
            movimento.IdContaCorrente,
            movimento.DataMovimento,
            movimento.TipoMovimento,
            movimento.Valor,
        });

        return result > 0;
    }
}
