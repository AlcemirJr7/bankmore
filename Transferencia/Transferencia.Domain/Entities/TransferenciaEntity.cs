namespace Transferencia.Domain.Entities;

public class TransferenciaEntity
{
    public string IdTransferencia { get; private set; } = string.Empty;
    public string IdContaCorrenteOrigem { get; private set; } = string.Empty;
    public string IdContaCorrenteDestino { get; private set; } = string.Empty;
    public DateTime DataMovimento { get; private set; }
    public decimal Valor { get; private set; }
    public string Status { get; private set; } = string.Empty;

    public TransferenciaEntity(string idContaCorrenteOrigem, string idContaCorrenteDestino, decimal valor, string status)
    {
        IdTransferencia = Guid.NewGuid().ToString();
        IdContaCorrenteOrigem = idContaCorrenteOrigem;
        IdContaCorrenteDestino = idContaCorrenteDestino;
        DataMovimento = DateTime.Now;
        Valor = valor;
        Status = status;
    }

    public void UpdateStatus(string status)
    {
        Status = status;
    }
}
