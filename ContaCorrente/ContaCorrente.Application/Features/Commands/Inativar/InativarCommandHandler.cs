using ContaCorrente.Application.Errors;
using ContaCorrente.Application.Features.Commands.Inativar.Validation;
using ContaCorrente.Domain.Repositories;
using Core.Response;
using Core.Security.Auth;
using MediatR;
using Serilog;

namespace ContaCorrente.Application.Features.Commands.Inativar;

public sealed class InativarCommandHandler(
        IContaCorrenteCommandRepository commandRepository,
        IContaCorrenteQueryRepository queryRepository,
        IAuthService authService,
        IInativarValidator validator) : IRequestHandler<InativarRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(InativarRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var conta = await queryRepository.BuscaPeloIdAsync(request.IdContaCorrente);

            var validationResult = validator.Validar(request, conta);

            if (!validationResult.IsSuccess)
                return validationResult;

            var credenciais = await queryRepository.BuscaCredenciaisPeloIdAsync(request.IdContaCorrente);

            var auth = authService.Autentica(new AutenticaInputModel
            {
                Senha = request.Senha,
                Hash = credenciais.Senha,
                Salt = credenciais.Salt
            });

            if (!auth.IsSuccess)
                return ApiResponse.Failure(auth.Error);

            await commandRepository.InativarAsync(conta!.IdContaCorrente);

            return ApiResponse.Success();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao inativar conta corrente.");
            return ApiResponse.Failure(AppErrors.Account.FailInactive);
        }
    }
}
