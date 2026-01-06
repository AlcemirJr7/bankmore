using Core.Errors;
using Core.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Core.DataAnnotations;

public class ValorMaiorZeroAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success;

        if (!value.MaiorQueZero())
            return new ValidationResult(JsonSerializer.Serialize(ValidationErrors.InvalidValue(value.ToStr())));

        return ValidationResult.Success;
    }
}
