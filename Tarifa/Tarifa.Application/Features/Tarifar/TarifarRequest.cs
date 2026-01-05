using Core.DataAnnotations;
using Core.Definitions;
using Core.Response;
using MediatR;

namespace Tarifa.Application.Features.Tarifar;

public sealed class TarifarRequest : IRequest<ApiResponse>
{
    [TamanhoValido(AtributosDefinitions.PrimaryKeyMaxLength)]
    public string IdContaCorrete { get; set; } = string.Empty;

    [DataValida]
    public string DataMovimento { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}
