using Core.Response;

namespace Core.Security.Auth;

public interface IAuthService
{
    ApiResponse Autentica(AutenticaInputModel input);
}
