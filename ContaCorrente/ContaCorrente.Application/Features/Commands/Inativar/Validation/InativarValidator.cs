using ContaCorrente.Domain.Errors;
using ContaCorrente.Domain.Models.Results;
using ContaCorrente.Domain.ValueObjects;
using Core.Response;

namespace ContaCorrente.Application.Features.Commands.Inativar.Validation;

public sealed class InativarValidator : IInativarValidator
{
    public ApiResponse Validar(InativarRequest input, ContaCorrenteResultModel? data)
    {
        if (data is null)
            return ApiResponse.Failure(DomainErrors.Account.Invalid);

        if (!FlagAtivo.IsAtivo(data.Ativo))
            return ApiResponse.Failure(DomainErrors.Account.Inactive);

        return ApiResponse.Success();
    }
}
