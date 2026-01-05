namespace Core.Idempotencia;

public record struct IdempotenciaConsts
{
    public const string HeaderKeyName = "Idempotencia-Key";
    public const int MaxLength = 37;
}
