using Core.Idempotencia;
using Core.Response;

namespace Core.Errors;

public readonly record struct CoreErrors
{
    public readonly record struct Idempotency
    {
        public static readonly ErrorDetails HeaderIdempotencia =
            new("HEADER_IDEMPOTENCIA", "Header 'Idempotencia-Key' é obrigatório");

        public static readonly ErrorDetails InvalidLength =
            new("INVALID_IDEMPOTENCIA_LENGTH", $"Idempotencia-Key deve ter no máximo {IdempotenciaConsts.MaxLength} caracteres.");
    }
}
