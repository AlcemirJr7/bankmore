using ContaCorrente.Application.Errors;
using ContaCorrente.Domain.Errors;
using ContaCorrente.Domain.Models.Inputs;
using ContaCorrente.Domain.Repositories;
using ContaCorrente.Domain.ValueObjects;
using Core.Response;
using MediatR;
using Serilog;

namespace ContaCorrente.Application.Features.Queries.ConsultaId;

public sealed class ConsultaIdQueryHandler(
    IContaCorrenteQueryRepository queryRepository) : IRequestHandler<ConsultaIdRequest, ApiResponse<ConsultaIdResponse>>
{
    public async Task<ApiResponse<ConsultaIdResponse>> Handle(ConsultaIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var buscaConta = new BuscaGenericaInputModel
            {
                idContaCorrente = request.IdContaCorrente,
                numero = request.NumeroConta
            };

            var result = await queryRepository.ConsultaIdAsync(buscaConta, ct: cancellationToken);

            if (result is null)
                return ApiResponse.Failure<ConsultaIdResponse>(DomainErrors.Account.Invalid);

            if (!FlagAtivo.IsAtivo(result.Ativo))
                return ApiResponse.Failure<ConsultaIdResponse>(DomainErrors.Account.Inactive);

            return ApiResponse.Success(new ConsultaIdResponse
            {
                IdContaCorrente = result.IdContaCorrente,
                Numero = result.Numero
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao consultar identificação da conta corrente.");
            return ApiResponse.Failure<ConsultaIdResponse>(AppErrors.Account.FailConsultId);
        }
    }
}
