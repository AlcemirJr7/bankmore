namespace Tarifa.Domain.Entities;

public class TarifaEntity
{
    public string IdTarifa { get; private set; } = string.Empty;
    public string IdContaCorrente { get; private set; } = string.Empty;
    public string DataMovimento { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }

    public TarifaEntity(string idContaCorrente, string dataMovimento, decimal valor)
    {
        IdTarifa = Guid.NewGuid().ToString();
        IdContaCorrente = idContaCorrente;
        DataMovimento = dataMovimento;
        Valor = valor;
    }
}
