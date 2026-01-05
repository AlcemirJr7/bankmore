using Core.Response;

namespace Tarifa.Domain.Errors;

public readonly record struct DomainErrors
{
    public readonly record struct Fee
    {
        public static readonly ErrorDetails InvalidValue =
            new("INVALID_VALUE", "Valor informado é inválido.");
    }
}
