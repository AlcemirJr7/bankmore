using Core.Response;

namespace ContaCorrente.Domain.Errors;

public readonly record struct DomainErrors
{
    public readonly record struct Account
    {
        public static readonly ErrorDetails Invalid =
            new("INVALID_ACCOUNT", "Conta corrente inválida.");

        public static readonly ErrorDetails Inactive =
            new("INACTIVE_ACCOUNT", "Conta corrente inativa.");

        public static readonly ErrorDetails AlreadyExists =
            new("ALREADY_EXISTS_ACCOUNT", "Já Existe uma conta com o documento informado.");

        public static readonly ErrorDetails WeakPassword =
            new("WEAK_PASSWORD", "Senha informada é muito fraca.");

        public static readonly ErrorDetails InvalidDocument =
            new("INVALID_DOCUMENT", "Documento informado é inválido.");
    }

    public readonly record struct Movement
    {
        public static readonly ErrorDetails InvalidValue =
            new("INVALID_VALUE", "Valor informado inválido.");

        public static readonly ErrorDetails InvalidType =
            new("INVALID_TYPE", "Tipo de movimentação inválida.");

        public static readonly ErrorDetails Invalid =
            new("INVALID_TYPE", "Movimentação inválida.");
    }
}
