using Core.Response;
using MediatR;
using Serilog;
using Tarifa.Application.Errors;
using Tarifa.Domain.Entities;
using Tarifa.Domain.Repositories;

namespace Tarifa.Application.Features.Tarifar;

public sealed class TarifarCommandHandler(
    ICommandRepository commandRepository)
    : IRequestHandler<TarifarRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(TarifarRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var tarifa = new TarifaEntity(request.IdContaCorrete, request.DataMovimento, request.Valor);

            var result = await commandRepository.CreateAsync(tarifa, cancellationToken);

            if (!result)
                return ApiResponse.Failure(AppErrors.Fee.FailCreate);

            return ApiResponse.Success();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao processar tarifa.");
            return ApiResponse.Failure(AppErrors.Fee.FailFee);
        }
    }
}
