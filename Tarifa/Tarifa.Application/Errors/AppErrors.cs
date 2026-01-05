using Core.Response;

namespace Tarifa.Application.Errors;

public readonly record struct AppErrors
{
    public readonly record struct Fee
    {
        public static readonly ErrorDetails FailCreate =
            new("FAIL_CREATE_FEE", "Não foi possível criar tarifa.");

        public static readonly ErrorDetails FailFee =
            new("FAIL_FEE", "Falha ao efetuar tarifação.");
    }
}
