using ContaCorrente.Domain.Errors;
using ContaCorrente.Domain.Models.Results;
using ContaCorrente.Domain.ValueObjects;
using Core.Response;
using Core.ValueObjects;

namespace ContaCorrente.Application.Features.Commands.Movimentar.Validation;

public sealed class CriarMovimentoValidator : ICriarMovimentoValidator
{
    public ApiResponse Validar(CriarMovimentoRequest input, ContaCorrenteResultModel? data)
    {
        if (data is null)
            return ApiResponse.Failure(DomainErrors.Account.Invalid);

        if (!FlagAtivo.IsAtivo(data.Ativo))
            return ApiResponse.Failure(DomainErrors.Account.Inactive);

        if (!TipoMovimento.IsValid(input.Tipo))
            return ApiResponse.Failure(DomainErrors.Movement.InvalidType);

        if (input.Valor <= 0)
            return ApiResponse.Failure(DomainErrors.Movement.InvalidValue);

        if (input.IdContaLogada != data?.IdContaCorrente && !TipoMovimento.IsCredito(input.Tipo))
            return ApiResponse.Failure(DomainErrors.Movement.Invalid);

        return ApiResponse.Success();
    }
}
