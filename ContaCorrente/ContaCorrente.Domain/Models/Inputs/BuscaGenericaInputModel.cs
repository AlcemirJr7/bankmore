namespace ContaCorrente.Domain.Models.Inputs;

public record BuscaGenericaInputModel(
    string? idContaCorrente = null,
    string? documento = null,
    int? numero = null);
