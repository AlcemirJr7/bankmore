using Core.DataAnnotations;

namespace Core.Idempotencia;

public class IdempotenciaEntity
{
    [TamanhoValido(37)]
    public string ChaveIdempotencia { get; set; } = string.Empty;
    public string Requisicao { get; set; } = string.Empty;
    public string Resultado { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string DataCriacao { get; set; } = string.Empty;
}