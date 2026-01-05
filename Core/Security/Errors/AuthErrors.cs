using Core.Response;

namespace Core.Security.Errors;

public readonly record struct AuthErrors
{
    public readonly record struct Login
    {
        public static readonly ErrorDetails UnauthorizedUser =
            new("USER_UNAUTHORIZED", "Usuário não autorizado.", ApiStatusCode.Forbidden);

        public static readonly ErrorDetails Invalid =
            new("INVALID_LOGIN", "Usuário inválido.", ApiStatusCode.Unauthorized);

        public static readonly ErrorDetails InvalidPassword =
            new("INVALID_PASSWORD", "Senha inválida.", ApiStatusCode.Unauthorized);

        public static readonly ErrorDetails Unauthorized =
            new("UNAUTHORIZED", "Token inválido ou não informado.", ApiStatusCode.Unauthorized);
    }

    public readonly record struct JWT
    {
        public static readonly string InvalidSettings =
            "As configurações de JWT são inválidas. Verifique o 'JwtSettings'.";

        public static readonly string InvalidToken =
            "O token JWT fornecido é inválido.";

        public static readonly string FailAuthentication =
            "Falha na autenticação do token JWT.";
    }
}
