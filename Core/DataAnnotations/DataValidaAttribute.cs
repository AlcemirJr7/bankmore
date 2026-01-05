using Core.Errors;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;

namespace Core.DataAnnotations;

public class DataValidaAttribute : ValidationAttribute
{
    private readonly string[] _formatos = {"dd/MM/yyyy", "dd/MM/yyyy HH:mm:ss"};

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success;
        
        foreach (var formato in _formatos)
        {
            if (!DateTime.TryParseExact(
                value.ToString(),
                formato,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _))
            {
                return new ValidationResult(
                    JsonSerializer.Serialize(ValidationErrors.InvalidDate(string.Join(", ", _formatos))));
            }
        }

        return ValidationResult.Success;
    }
}
