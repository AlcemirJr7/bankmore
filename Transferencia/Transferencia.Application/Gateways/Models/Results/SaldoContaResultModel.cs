using Core.Extensions;

namespace Transferencia.Application.Gateways.Models.Results;

public record SaldoContaResultModel
{
    public string IdContaCorrente { get; set; } = string.Empty;
    public int Numero { get; set; } = 0;
    public string DataHoraConsulta { get; set; } = DateTime.Now.BrStr();
    public decimal Saldo { get; set; } = 0;
}
