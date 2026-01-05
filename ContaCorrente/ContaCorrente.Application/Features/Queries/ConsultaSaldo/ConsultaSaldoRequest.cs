using Core.Response;
using MediatR;

namespace ContaCorrente.Application.Features.Queries.ConsultaSaldo;

public sealed class ConsultaSaldoRequest : IRequest<ApiResponse<ConsultaSaldoResponse>>
{
    public string IdContaCorrente { get; set; } = string.Empty;
}
