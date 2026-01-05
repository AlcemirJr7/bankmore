using ContaCorrente.Domain.Models.Inputs;
using ContaCorrente.Domain.Models.Results;
using ContaCorrente.Domain.Repositories;
using Core.Response;
using Core.Security.Auth;
using Core.Security.Errors;
using Core.Security.Jwt;
using MediatR;
using Serilog;

namespace ContaCorrente.Application.Security.Login;

public sealed class LoginCommandHandler(
        IJwtTokenService jwtTokenGenerator,
        IContaCorrenteQueryRepository queryRepository,
        IAuthService authService) : IRequestHandler<LoginRequest, ApiResponse<LoginResponse>>
{
    public async Task<ApiResponse<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            ContaCorrenteResultModel? conta = null;

            var buscaConta = new BuscaGenericaInputModel(
                documento: request.Documento,
                numero: request.NumeroConta);

            conta = await queryRepository.BuscaContaCorrenteAsync(buscaConta, cancellationToken);

            if (conta is null)
                return ApiResponse.Failure<LoginResponse>(AuthErrors.Login.UnauthorizedUser);

            var credenciais = await queryRepository.BuscaCredenciaisPeloIdAsync(conta.IdContaCorrente);

            var auth = authService.Autentica(new AutenticaInputModel
            {
                Senha = request.Senha,
                Hash = credenciais.Senha,
                Salt = credenciais.Salt
            });

            if (!auth.IsSuccess)
                return ApiResponse.Failure<LoginResponse>(auth.Error);

            var token = jwtTokenGenerator.GenerateToken(conta.IdContaCorrente);

            return ApiResponse.Success(new LoginResponse(token));
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao realizar login.");
            return ApiResponse.Failure<LoginResponse>(AuthErrors.Login.Invalid);
        }
    }
}
