namespace ContaCorrente.Application.Features.Commands.Movimentar.Service;

public record CriarMovimentoInputModel
{
    public string IdContaLogada { get; set; } = string.Empty;
    public int NumeroConta { get; init; } = 0;
    public decimal Valor { get; init; } = 0;
    public string Tipo { get; init; } = string.Empty;
}
