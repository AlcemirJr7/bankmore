using Core.Messengers.Models;
using Core.Response;
using Core.ValueObjects;
using MediatR;
using Serilog;
using Transferencia.Application.Errors;
using Transferencia.Application.Features.TransferirInterno.Validation;
using Transferencia.Application.Gateways;
using Transferencia.Application.Gateways.Models.Inputs;
using Transferencia.Application.Messengers;
using Transferencia.Domain.Entities;
using Transferencia.Domain.Repositories;
using Transferencia.Domain.ValueObjects;

namespace Transferencia.Application.Features.TransferirInterno;

public sealed class TransferirInternoCommandHandler(
        IContaCorrenteApiGateway contaCorrenteApiGateway,
        ITransferenciaCommandRepository commandRepository,
        ITransferirInternoValidator transferirInternoValidator,
        ITransferenciasRealizadasProducerMessenger transferenciasRealizadasMessenger)
    : IRequestHandler<TransferirInternoRequest, ApiResponse>
{
    private TransferenciaEntity? transferencia;

    public async Task<ApiResponse> Handle(TransferirInternoRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var saldoContaOrigem = await contaCorrenteApiGateway.ConsultaSaldoAsync(request.IdContaLogada, cancellationToken);

            var validationResult = transferirInternoValidator.Validar(request, saldoContaOrigem.Data);

            if (!validationResult.IsSuccess)
                return validationResult;

            // Busca a identificação da conta corrente destino para gravar a transferencia
            var contaDestino = await contaCorrenteApiGateway.ConsultaIdAsync(
                numeroConta: request.NumeroContaDestino,
                ct: cancellationToken);

            if (!contaDestino.IsSuccess)
                return ApiResponse.Failure(contaDestino.Error);

            transferencia = new TransferenciaEntity(
                request.IdContaLogada,
                contaDestino.Data!.IdContaCorrente,
                request.Valor,
                StatusTransacao.PENDENTE);

            var result = await commandRepository.CriarAsync(transferencia, cancellationToken);

            if (!result)
                return ApiResponse.Failure(AppErrors.Transfer.FailTransfer);

            // O débito é feito com base no id da conta corrente que esta logada.
            var debitoResult = await DebitarAsync(
                new CriaMovimentoInputModel
                {
                    Tipo = TipoMovimento.Debito,
                    Valor = request.Valor
                }, cancellationToken);

            if (!debitoResult.IsSuccess)
                return debitoResult;

            var creditoResult = await CreditarAsync(
                new CriaMovimentoInputModel
                {
                    NumeroConta = request.NumeroContaDestino,
                    Tipo = TipoMovimento.Credito,
                    Valor = request.Valor
                }, cancellationToken);

            if (!creditoResult.IsSuccess)
            {
                var estornoResult = await EstornarAsync(
                    new CriaMovimentoInputModel
                    {
                        NumeroConta = saldoContaOrigem.Data!.Numero,
                        Tipo = TipoMovimento.Credito,
                        Valor = request.Valor
                    }, cancellationToken);

                if (!estornoResult.IsSuccess)
                    return estornoResult;

                return creditoResult;
            }

            await AlteraStatusAsync(StatusTransacao.PROCESSADO, cancellationToken);

            await transferenciasRealizadasMessenger.EnviarAsync(
                new TransferenciasRealizadasMessage
                {
                    IdTransferencia = transferencia.IdTransferencia,
                    IdContaCorrente = request.IdContaLogada,
                    Valor = request.Valor
                });

            return ApiResponse.Success();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao processar transferência interna para conta.");
            return ApiResponse.Failure(AppErrors.Transfer.FailTransfer);
        }
    }

    private async Task AlteraStatusAsync(string status, CancellationToken ct)
    {
        if (transferencia is null)
            return;

        transferencia.UpdateStatus(status);
        await commandRepository.AlteraStatusAsync(transferencia, ct);
    }

    private async Task<ApiResponse> DebitarAsync(CriaMovimentoInputModel input, CancellationToken ct)
    {
        try
        {
            var result = await contaCorrenteApiGateway.CriaMovimentoAsync(
                input,
                chaveIdempotencia: $"{transferencia?.IdTransferencia}D",
                ct: ct);

            if (!result.IsSuccess)
            {
                await AlteraStatusAsync(StatusTransacao.ERRO_DEBITO, ct);
                return result;
            }

            return ApiResponse.Success(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Erro ao tentar debitar.");
            return ApiResponse.Failure(AppErrors.Transfer.FailTransfer);
        }
    }

    private async Task<ApiResponse> CreditarAsync(CriaMovimentoInputModel input, CancellationToken ct)
    {
        try
        {
            var result = await contaCorrenteApiGateway.CriaMovimentoAsync(
                input,
                chaveIdempotencia: $"{transferencia?.IdTransferencia}C",
                ct: ct);

            if (!result.IsSuccess)
            {
                await AlteraStatusAsync(StatusTransacao.ERRO_CREDITO, ct);
                return ApiResponse.Failure(result.Error);
            }

            return ApiResponse.Success(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Erro ao tentar creditar.");
            return ApiResponse.Failure(AppErrors.Transfer.FailTransfer);
        }
    }

    private async Task<ApiResponse> EstornarAsync(CriaMovimentoInputModel input, CancellationToken ct)
    {
        try
        {
            var result = await contaCorrenteApiGateway.CriaMovimentoAsync(
                input,
                chaveIdempotencia: $"{transferencia?.IdTransferencia}E",
                ct: ct);

            if (!result.IsSuccess)
            {
                await AlteraStatusAsync(StatusTransacao.ERRO_ESTORNO, ct);
                return ApiResponse.Failure(result.Error);
            }

            await AlteraStatusAsync(StatusTransacao.ESTORNADO, ct);

            return ApiResponse.Success(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Erro ao tentar estornar.");
            return ApiResponse.Failure(AppErrors.Transfer.FailTransfer);
        }
    }
}
