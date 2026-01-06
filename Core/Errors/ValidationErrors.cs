using Core.Response;

namespace Core.Errors;

public readonly record struct ValidationErrors
{
    public static ErrorDetails MaxLength(int value) =>
            new("MAX_LENGTH_VALIDATION", $"Tamanho máximo deve ser: {value}.");

    public static ErrorDetails InvalidDate(string value) =>
            new("INVALID_DATE_VALIDATION", $"Data inválida. Formato esperado: {value}.");

    public static ErrorDetails InvalidValue(string value) =>
            new("INVALID_VALUE_VALIDATION", $"Valor informado deve ser maior que 0. Valor informado: {value}.");

    public static ErrorDetails InvalidType(string value) =>
            new("INVALID_TYPE_VALIDATION", $"Tipo informado inválido. Tipo informado: {value}.");
}
