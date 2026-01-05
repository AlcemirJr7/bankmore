using ContaCorrente.Domain.Models.Inputs;
using ContaCorrente.Domain.Models.Results;
using ContaCorrente.Domain.Repositories;
using Core.Infrastructure.Database;
using Dapper;

namespace ContaCorrente.Infrastructure.Repositories;

public class ContaCorrenteQueriesRepository(IDbConnectionFactory connectionFactory) : IContaCorrenteQueryRepository
{

    private const string SqlContaCorrenteBase = @"select c.idcontacorrente,
                                                         c.numero,                                                   
                                                         c.documento,
                                                         c.nome,
                                                         c.ativo
                                                    from contacorrente c
                                                   where 1 = 1";

    public async Task<ContaCorrenteResultModel?> BuscaPeloIdAsync(string idContaCorrente, CancellationToken ct = default)
    {
        const string sql = @$"{SqlContaCorrenteBase}
                              and c.idcontacorrente = @Id";

        using var conn = connectionFactory.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<ContaCorrenteResultModel>(sql, new { Id = idContaCorrente });
    }

    public async Task<int> BuscaNovoNumeroContaAsync(CancellationToken ct = default)
    {
        const string sql = @$"select ifnull(max(numero), 0) + 1 as numero from contacorrente";

        using var conn = connectionFactory.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<int>(sql, ct);
    }

    public async Task<ConsultaSaldoResultModel?> ConsultaSaldoAsync(string idContaCorrente, CancellationToken ct = default)
    {
        string sql = @"select c.idcontacorrente,
                               c.numero,
                               c.ativo,
                               ifnull(sum(case
                                           when m.tipomovimento = 'D' then (m.valor * -1)
		       		                       else m.valor
	       		                       end), 0) as saldo
                         from contacorrente c
                    left join movimento m on (m.idcontacorrente = c.idcontacorrente)
                    where c.idcontacorrente = @Id
                    group by c.numero, c.nome, c.ativo";

        using var conn = connectionFactory.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<ConsultaSaldoResultModel>(sql, new { Id = idContaCorrente });
    }

    public async Task<ContaCorrenteResultModel?> BuscaPeloDocumentoAsync(string documento, CancellationToken ct = default)
    {
        const string sql = @$"{SqlContaCorrenteBase}
                              and c.documento = @Documento";

        using var conn = connectionFactory.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<ContaCorrenteResultModel>(sql, new { Documento = documento });
    }

    public async Task<ContaCorrenteResultModel?> BuscaPeloNumeroAsync(int numero, CancellationToken ct = default)
    {
        const string sql = @$"{SqlContaCorrenteBase} 
                               and c.numero = @Numero";

        using var conn = connectionFactory.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<ContaCorrenteResultModel>(sql, new { Numero = numero });
    }

    public async Task<CredenciaisResultModel> BuscaCredenciaisPeloIdAsync(string idContaCorrente, CancellationToken ct = default)
    {
        const string sql = @$"select c.idcontacorrente,
	                                 c.senha,
                                     c.salt
                                from contacorrente c
                               where c.idcontacorrente = @Id";

        using var conn = connectionFactory.CreateConnection();

        return await conn.QueryFirstAsync<CredenciaisResultModel>(sql, new { Id = idContaCorrente });
    }

    public async Task<ConsultaIdResultModel?> ConsultaIdAsync(BuscaGenericaInputModel input, CancellationToken ct = default)
    {
        string filtro = string.Empty;

        var parameters = new DynamicParameters();

        if (input.idContaCorrente is not null)
        {
            parameters.Add("Id", input.idContaCorrente);
            filtro = "where c.idcontacorrente = @Id";
        }

        if (input.numero.HasValue && input.numero > 0)
        {
            parameters.Add("Numero", input.numero);
            filtro = "where c.numero = @Numero";
        }

        string sql = @$"select c.idcontacorrente,
                               c.numero,
                               c.ativo
                         from contacorrente c
                         {filtro}";

        using var conn = connectionFactory.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<ConsultaIdResultModel>(sql, parameters);
    }

    public async Task<ContaCorrenteResultModel?> BuscaContaCorrenteAsync(BuscaGenericaInputModel input, CancellationToken ct = default)
    {
        string filtro = string.Empty;

        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(input.idContaCorrente))
        {
            parameters.Add("Id", input.idContaCorrente);
            filtro = "and c.idcontacorrente = @Id";
        }

        if (input.numero.HasValue && input.numero > 0)
        {
            parameters.Add("Numero", input.numero.Value);
            filtro = "and c.numero = @Numero";
        }

        if (!string.IsNullOrEmpty(input.documento))
        {
            parameters.Add("Documento", input.documento);
            filtro = "and c.documento = @Documento";
        }

        string sql = @$"{SqlContaCorrenteBase} {filtro}";

        using var conn = connectionFactory.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<ContaCorrenteResultModel>(sql, parameters);
    }
}
