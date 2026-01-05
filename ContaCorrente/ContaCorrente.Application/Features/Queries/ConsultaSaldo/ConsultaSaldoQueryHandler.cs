using ContaCorrente.Application.Errors;
using ContaCorrente.Domain.Errors;
using ContaCorrente.Domain.Repositories;
using ContaCorrente.Domain.ValueObjects;
using Core.Response;
using MediatR;
using Serilog;

namespace ContaCorrente.Application.Features.Queries.ConsultaSaldo;

public sealed class ConsultaSaldoQueryHandler(IContaCorrenteQueryRepository queryRepository) : IRequestHandler<ConsultaSaldoRequest, ApiResponse<ConsultaSaldoResponse>>
{
    public async Task<ApiResponse<ConsultaSaldoResponse>> Handle(
        ConsultaSaldoRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await queryRepository.ConsultaSaldoAsync(request.IdContaCorrente, ct: cancellationToken);

            if (result is null)
                return ApiResponse.Failure<ConsultaSaldoResponse>(DomainErrors.Account.Invalid);

            if (!FlagAtivo.IsAtivo(result.Ativo))
                return ApiResponse.Failure<ConsultaSaldoResponse>(DomainErrors.Account.Inactive);

            return ApiResponse.Success(
                new ConsultaSaldoResponse
                {
                    IdContaCorrente = result.IdContaCorrente,
                    Numero = result.Numero,
                    Saldo = result.Saldo
                });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao consultar saldo da conta corrente.");
            return ApiResponse.Failure<ConsultaSaldoResponse>(AppErrors.Account.FailConsultBalance);
        }
    }
}
