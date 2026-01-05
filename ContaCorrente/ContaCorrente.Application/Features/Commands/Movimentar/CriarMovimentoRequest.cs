using Core.Abstractions;
using Core.Response;
using MediatR;

namespace ContaCorrente.Application.Features.Commands.Movimentar;

public sealed class CriarMovimentoRequest : RequestBase, IRequest<ApiResponse<CriarMovimentoResponse>>
{
    public int NumeroConta { get; init; } = 0;
    public decimal Valor { get; init; } = 0;
    public string Tipo { get; init; } = string.Empty;
}
