using ContaCorrente.Domain.Errors;
using ContaCorrente.Domain.Models.Results;
using Core.Response;

namespace ContaCorrente.Application.Features.Commands.Cadastrar.Validation;

public sealed class CadastrarValidator : ICadastrarValidator
{
    public ApiResponse Validar(CadastrarRequest input, ContaCorrenteResultModel? data)
    {
        if (data is not null)
            return ApiResponse.Failure(DomainErrors.Account.AlreadyExists);

        if (!IsDocumentoValido(input.Documento))
            return ApiResponse.Failure(DomainErrors.Account.InvalidDocument);

        if (!IsSenhaStrong(input.Senha))
            return ApiResponse.Failure(DomainErrors.Account.WeakPassword);

        return ApiResponse.Success();
    }

    private bool IsDocumentoValido(string documento) => documento.Length == 11;

    private bool IsSenhaStrong(string senha) => senha.Length >= 8;
}
