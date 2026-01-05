using ContaCorrente.Domain.ValueObjects;

namespace ContaCorrente.Domain.Entities;

public class ContaCorrenteEntity
{
    public string IdContaCorrente { get; private set; }

    public int Numero { get; private set; }

    public string Documento { get; private set; } = string.Empty;

    public string Nome { get; private set; } = string.Empty;

    public int Ativo { get; private set; }

    public string Senha { get; private set; } = string.Empty;

    public string Salt { get; private set; } = string.Empty;

    public ContaCorrenteEntity(int numero, string documento, string nome, string senha, string salt)
    {
        IdContaCorrente = Guid.NewGuid().ToString();
        Numero = numero;
        Documento = documento;
        Nome = nome;
        Senha = senha;
        Ativo = FlagAtivo.Ativo;
        Salt = salt;
    }
}
