namespace Core.Messengers.Models;

public class TarifasRealizadasMessage
{
    public string IdTarifa { get; set; } = string.Empty;
    public string IdContaCorrente { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}
