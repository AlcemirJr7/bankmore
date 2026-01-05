namespace Core.Messengers.Models;

public class TransferenciasRealizadasMessage
{
    public string IdTransferencia { get; set; } = string.Empty;
    public string IdContaCorrente { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}
