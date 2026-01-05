using Core.Response;
using MediatR;

namespace ContaCorrente.Application.Features.Queries.ConsultaId;

public sealed class ConsultaIdRequest : IRequest<ApiResponse<ConsultaIdResponse>>
{
    public string? IdContaCorrente { get; set; } = string.Empty;
    public int? NumeroConta { get; set; } = 0;
}
