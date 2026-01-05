using ContaCorrente.Application.Errors;
using ContaCorrente.Application.Features.Commands.Movimentar.Service;
using Core.Response;
using MediatR;
using Serilog;

namespace ContaCorrente.Application.Features.Commands.Movimentar;

public sealed class CriarMovimentoCommandHandler(ICriarMovimentoService criarMovimentoService)
    : IRequestHandler<CriarMovimentoRequest, ApiResponse<CriarMovimentoResponse>>
{
    public async Task<ApiResponse<CriarMovimentoResponse>> Handle(
        CriarMovimentoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await criarMovimentoService.CriaMovimentoAsync(request, cancellationToken);

            if (!result.IsSuccess)
                return ApiResponse.Failure<CriarMovimentoResponse>(result.Error);

            return ApiResponse.SuccessCreated(result.Data!);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao realizar movimentação.");
            return ApiResponse.Failure<CriarMovimentoResponse>(AppErrors.Movement.FailMovement);
        }
    }
}
