using Core.Errors;
using Core.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Core.DataAnnotations;

public class TamanhoValidoAttribute : ValidationAttribute
{
    private readonly int _tamanho;

    public TamanhoValidoAttribute(int tamanho)
    {
        _tamanho = tamanho;
    }

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success;

        if (value.ToStr().Length > _tamanho)
            return new ValidationResult(JsonSerializer.Serialize(ValidationErrors.MaxLength(_tamanho)));
        
        return ValidationResult.Success;
    }
}
