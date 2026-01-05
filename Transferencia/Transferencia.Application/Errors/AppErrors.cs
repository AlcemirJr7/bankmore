using Core.Response;

namespace Transferencia.Application.Errors;

public readonly record struct AppErrors
{
    public readonly record struct Transfer
    {
        public static readonly ErrorDetails InsufficientBalance =
            new("INSUFFICIENT_BALANCE", "Saldo insuficiente.");

        public static readonly ErrorDetails FailTransfer =
            new("FAIL_TRANSFER", "Falha ao efetuar transferencia.");
    }
}
