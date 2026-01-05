using Core.Response;
using Core.Security.Auth;
using Core.Security.Errors;

namespace Core.Infrastructure.Security.Auth;

public sealed class AuthService(ILogin login) : IAuthService
{
    public ApiResponse Autentica(AutenticaInputModel input)
    {
        var isSenhaValida = login.ValidaSenha(input.Senha, input.Hash, input.Salt);

        if (!isSenhaValida)
            return ApiResponse.Failure(AuthErrors.Login.InvalidPassword);

        return ApiResponse.Success();
    }
}
