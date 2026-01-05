using Core.Extensions;

namespace ContaCorrente.Application.Features.Queries.ConsultaSaldo;

public sealed class ConsultaSaldoResponse
{
    public string IdContaCorrente { get; set; } = string.Empty;
    public int Numero { get; set; } = 0;
    public DateTime DataHoraConsulta { get; set; } = DateTime.Now.Br();
    public decimal Saldo { get; set; } = 0;
}
