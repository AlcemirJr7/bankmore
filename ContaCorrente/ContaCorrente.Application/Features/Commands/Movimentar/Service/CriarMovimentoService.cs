using ContaCorrente.Application.Errors;
using ContaCorrente.Application.Features.Commands.Movimentar.Validation;
using ContaCorrente.Domain.Entities;
using ContaCorrente.Domain.Models.Inputs;
using ContaCorrente.Domain.Repositories;
using Core.Response;
using Serilog;

namespace ContaCorrente.Application.Features.Commands.Movimentar.Service;

public sealed class CriarMovimentoService(
    IContaCorrenteQueryRepository queryRepository,
    ICriarMovimentoValidator validator,
    IContaCorrenteCommandRepository commandRepository)
    : ICriarMovimentoService
{
    public async Task<ApiResponse<CriarMovimentoResponse>> CriaMovimentoAsync(
        CriarMovimentoRequest request, CancellationToken ct)
    {
        try
        {
            var buscaConta = new BuscaGenericaInputModel(numero: request.NumeroConta);

            var conta = await queryRepository.BuscaContaCorrenteAsync(buscaConta, ct);

            var validationResult = validator.Validar(request, conta);

            if (!validationResult.IsSuccess)
                return ApiResponse.Failure<CriarMovimentoResponse>(validationResult.Error);

            var movimento = new MovimentoEntity(conta!.IdContaCorrente, request.Tipo, request.Valor);

            await commandRepository.MovimentarAsync(movimento, ct);

            return ApiResponse.Success(new CriarMovimentoResponse
            {
                IdMovimento = movimento.IdMovimento
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao realizar movimentação.");
            return ApiResponse.Failure<CriarMovimentoResponse>(AppErrors.Movement.FailMovement);
        }
    }
}
