namespace ContaCorrente.Domain.Models.Results;

public record ContaCorrenteResultModel
{
    public string IdContaCorrente { get; set; } = string.Empty;
    public int Numero { get; set; } = 0;
    public string Documento { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public int Ativo { get; set; }
}