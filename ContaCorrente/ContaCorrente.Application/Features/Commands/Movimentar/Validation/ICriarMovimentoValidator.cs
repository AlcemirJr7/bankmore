using ContaCorrente.Domain.Models.Results;
using Core.Abstractions;
using Core.Response;

namespace ContaCorrente.Application.Features.Commands.Movimentar.Validation;

public interface ICriarMovimentoValidator : IValidator<CriarMovimentoRequest, ApiResponse, ContaCorrenteResultModel>
{
}
