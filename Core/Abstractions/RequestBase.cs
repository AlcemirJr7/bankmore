using Core.DataAnnotations;
using Core.Definitions;
using System.Text.Json.Serialization;

namespace Core.Abstractions;

public abstract class RequestBase
{
    [JsonIgnore]
    [TamanhoValido(AtributosDefinitions.PrimaryKeyMaxLength)]
    public string ChaveIdempotencia { get; set; } = string.Empty;

    [JsonIgnore]
    [TamanhoValido(AtributosDefinitions.PrimaryKeyMaxLength)]
    public string IdContaLogada { get; set; } = string.Empty;
}
