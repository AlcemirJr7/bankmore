namespace Transferencia.Domain.ValueObjects;

public record struct StatusTransacao
{
    public const string PENDENTE = "PENDENTE";
    public const string ERRO_DEBITO = "ERRO_DEBITO";
    public const string ERRO_CREDITO = "ERRO_CREDITO";
    public const string ESTORNADO = "ESTORNADO";
    public const string ERRO_ESTORNO = "ERRO_ESTORNO";
    public const string PROCESSADO = "PROCESSADO";

    public static bool IsValid(string status)
    {
        return status switch
        {
            PENDENTE => true,
            ERRO_DEBITO => true,
            ERRO_CREDITO => true,
            ESTORNADO => true,
            ERRO_ESTORNO => true,
            PROCESSADO => true,
            _ => false
        };
    }
}
