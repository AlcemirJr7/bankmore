using Core.Extensions;

namespace ContaCorrente.Domain.Models.Results;

public record ConsultaSaldoResultModel
{
    public string IdContaCorrente { get; set; } = string.Empty;
    public int Numero { get; set; }
    public int Ativo { get; set; }
    public DateTime DataHoraConsulta { get; set; } = DateTime.Now.Br();
    public decimal Saldo { get; set; } = 0;
}
