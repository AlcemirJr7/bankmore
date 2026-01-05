using Core.Response;

namespace ContaCorrente.Application.Errors;

public readonly record struct AppErrors
{
    public readonly record struct Account
    {
        public static readonly ErrorDetails FailCreate =
            new("FAIL_CREATE_ACCOUNT", "Falha ao cadastrar conta corrente.");

        public static readonly ErrorDetails FailConsultBalance =
            new("FAIL_CONSULT_BALANCE", "Falha ao consultar saldo.");

        public static readonly ErrorDetails FailInactive =
            new("FAIL_INACTIVE_ACCOUNT", "Falha ao inativar conta.");

        public static readonly ErrorDetails FailConsultId =
            new("FAIL_CONSULT_ID_ACCOUNT", "Falha ao consultar identificação da conta corrente.");
    }

    public readonly record struct Movement
    {
        public static readonly ErrorDetails FailConsult =
            new("FAIL_CONSULT_MOVEMENT", "Falha ao buscar movimento.");

        public static readonly ErrorDetails FailMovement =
            new("FAIL_MOVEMENT", "Falha ao movimentar.");
    }
}
