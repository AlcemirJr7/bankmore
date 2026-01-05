namespace Transferencia.Application.Gateways.Models.Inputs;

public record CriaMovimentoInputModel
{
    public int NumeroConta { get; set; } = 0;
    public decimal Valor { get; set; } = 0;
    public string Tipo { get; set; } = string.Empty;
}
