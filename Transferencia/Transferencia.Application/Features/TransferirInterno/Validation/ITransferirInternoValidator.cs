using Core.Abstractions;
using Core.Response;
using Transferencia.Application.Gateways.Models.Results;

namespace Transferencia.Application.Features.TransferirInterno.Validation;

public interface ITransferirInternoValidator : IValidator<TransferirInternoRequest, ApiResponse, SaldoContaResultModel>
{
}
