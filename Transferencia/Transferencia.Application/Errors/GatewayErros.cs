using Core.Response;

namespace Transferencia.Application.Errors;

public readonly record struct GatewayErros
{
    public readonly record struct ContaCorrenteApi
    {
        public static readonly ErrorDetails FailConsultBalanceRequest =
            new("FAIL_CONSULT_BALANCE_REQUEST", "Falha ao fazer request para consultar saldo.");

        public static readonly ErrorDetails FailDebitRequest =
            new("FAIL_DEBIT_REQUEST", "Falha ao fazer request para debitar.");

        public static readonly ErrorDetails FailMovementRequest =
            new("FAIL_MOVEMENT_REQUEST", "Falha ao fazer request para movimentar.");

        public static readonly ErrorDetails FailEffectiveRequest =
            new("FAIL_EFFECTIVE_REQUEST", "Falha ao fazer request para efetivar movimento.");

        public static readonly ErrorDetails FailConsultIdRequest =
            new("FAIL_CONSULT_ID_REQUEST", "Falha ao fazer request para consultar identificação da conta.");
    }
}
