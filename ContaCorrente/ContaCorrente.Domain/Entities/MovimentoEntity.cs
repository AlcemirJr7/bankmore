using Core.Extensions;

namespace ContaCorrente.Domain.Entities;

public class MovimentoEntity
{
    public string IdMovimento { get; private set; } = string.Empty;

    public string IdContaCorrente { get; private set; } = string.Empty;

    public string DataMovimento { get; private set; }

    public string TipoMovimento { get; private set; }

    public decimal Valor { get; private set; }

    public MovimentoEntity(string idContaCorrente, string tipoMovimento, decimal valor)
    {
        IdMovimento = Guid.NewGuid().ToString();
        IdContaCorrente = idContaCorrente;
        DataMovimento = DateTime.Now.BrStr();
        TipoMovimento = tipoMovimento;
        Valor = valor;
    }
}
