using Core.Response;
using Transferencia.Application.Errors;
using Transferencia.Application.Gateways.Models.Results;

namespace Transferencia.Application.Features.TransferirInterno.Validation;

public sealed class TransferirInternoValidator : ITransferirInternoValidator
{
    public ApiResponse Validar(TransferirInternoRequest input, SaldoContaResultModel? data)
    {
        if (data is null)
            return ApiResponse.Failure(AppErrors.Transfer.FailTransfer);

        if (data?.Numero == input.NumeroContaDestino)
            return ApiResponse.Failure(AppErrors.Transfer.FailTransfer);

        if (input.Valor > data?.Saldo)
            return ApiResponse.Failure(AppErrors.Transfer.InsufficientBalance);

        return ApiResponse.Success();
    }
}
