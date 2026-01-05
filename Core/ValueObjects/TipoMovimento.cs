namespace Core.ValueObjects;

public readonly record struct TipoMovimento
{
    public const string Debito = "D";
    public const string Credito = "C";

    public static bool IsValid(string tipo) => tipo switch { Debito or Credito => true, _ => false };

    public static bool IsCredito(string tipo) => tipo switch { Credito => true, _ => false };
}
