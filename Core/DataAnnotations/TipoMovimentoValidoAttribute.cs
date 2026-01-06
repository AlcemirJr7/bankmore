using Core.Errors;
using Core.Extensions;
using Core.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Core.DataAnnotations;

public class TipoMovimentoValidoAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (!TipoMovimento.IsValid(value.ToStr()))
            return new ValidationResult(JsonSerializer.Serialize(ValidationErrors.InvalidType(value.ToStr())));

        return ValidationResult.Success;
    }
}
