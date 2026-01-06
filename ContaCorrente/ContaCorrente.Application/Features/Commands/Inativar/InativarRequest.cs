using Core.DataAnnotations;
using Core.Definitions;
using Core.Response;
using MediatR;

namespace ContaCorrente.Application.Features.Commands.Inativar;

public sealed class InativarRequest : IRequest<ApiResponse>
{
    [TamanhoValido(AtributosDefinitions.DocumentoMaxLength)]
    public string IdContaCorrente { get; init; } = string.Empty;

    [TamanhoValido(AtributosDefinitions.SenhaHashMaxLength)]
    public string Senha { get; init; } = string.Empty;
}
