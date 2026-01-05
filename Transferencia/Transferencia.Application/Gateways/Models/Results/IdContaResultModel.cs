namespace Transferencia.Application.Gateways.Models.Results;

public record IdContaResultModel
{
    public string IdContaCorrente { get; set; } = string.Empty;
    public int Numero { get; set; } = 0;
}
