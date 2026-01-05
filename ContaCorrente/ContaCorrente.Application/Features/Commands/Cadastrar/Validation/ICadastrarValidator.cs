using ContaCorrente.Domain.Models.Results;
using Core.Abstractions;
using Core.Response;

namespace ContaCorrente.Application.Features.Commands.Cadastrar.Validation;

public interface ICadastrarValidator : IValidator<CadastrarRequest, ApiResponse, ContaCorrenteResultModel>
{
}
