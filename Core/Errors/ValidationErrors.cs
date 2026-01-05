using Core.Response;

namespace Core.Errors;

public readonly record struct ValidationErrors
{
    public static ErrorDetails MaxLength(int value) =>
            new("MAX_LENGTH_VALIDATION", $"Tamanho máximo deve ser: {value}.");

    public static ErrorDetails InvalidDate(string value) =>
            new("INVALID_DATE_VALIDATION", $"Data inválida. Formato esperado: {value}.");
}
