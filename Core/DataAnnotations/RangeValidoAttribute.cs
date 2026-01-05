using Core.Extensions;
using Core.Response;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Core.DataAnnotations;

public partial class RangeValidoAttribute : ValidationAttribute
{
    private decimal _minValue;
    private decimal _maxValue;

    public RangeValidoAttribute(decimal minValue = 0, decimal maxValue = decimal.MaxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
    }

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success;

        if (value.ToDecimalOrNull() < _minValue)
        {
            var resultError = new ErrorDetails
            (
                Type: "INVALID_RANGE",
                Message: $"Valor menor que o mínimo. Mínimo esperado: {_minValue}",
                StatusCode: ApiStatusCode.BadRequest
            );

            return new ValidationResult(
                JsonSerializer.Serialize(resultError));
        }

        if (value.ToDecimalOrNull() > _maxValue)
        {
            var resultError = new ErrorDetails
            (
                Type: "INVALID_RANGE",
                Message: $"Valor maior que o máximo. Máximo esperado: {_maxValue}",
                StatusCode: ApiStatusCode.BadRequest
            );

            return new ValidationResult(
                JsonSerializer.Serialize(resultError));
        }

        return ValidationResult.Success;
    }
}
