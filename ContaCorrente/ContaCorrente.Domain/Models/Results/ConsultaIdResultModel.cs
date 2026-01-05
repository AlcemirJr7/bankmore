namespace ContaCorrente.Domain.Models.Results;

public record ConsultaIdResultModel
{
    public string IdContaCorrente { get; set; } = string.Empty;
    public int Numero { get; set; } = 0;
    public int Ativo { get; set; } = 0;
}
