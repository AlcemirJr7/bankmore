namespace ContaCorrente.Domain.Models.Results;

public record CredenciaisResultModel
{
    public string IdContaCorrente { get; init; } = string.Empty;
    public string Senha { get; init; } = string.Empty;
    public string Salt { get; init; } = string.Empty;
}
