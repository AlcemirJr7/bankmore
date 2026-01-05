namespace ContaCorrente.Domain.ValueObjects;

public readonly record struct FlagAtivo
{
    public const int Inativo = 0;
    public const int Ativo = 1;

    public static bool IsAtivo(int flag) => flag switch { 1 => true, _ => false };
}
