using ContaCorrente.Domain.Models.Results;
using Core.Abstractions;
using Core.Response;

namespace ContaCorrente.Application.Features.Commands.Inativar.Validation;

public interface IInativarValidator : IValidator<InativarRequest, ApiResponse, ContaCorrenteResultModel>
{
}
