namespace Core.Extensions;

public static class ObjectExtensions
{
    public static int? ToIntOrNull(this object? value)
    {
        if (value is null)
            return null;

        if (value is int i)
            return i;

        if (int.TryParse(value.ToString(), out var result))
            return result;

        return null;
    }

    public static decimal? ToDecimalOrNull(this object? value)
    {
        if (value == null)
            return null;

        if (value is decimal i)
            return i;

        if (decimal.TryParse(value.ToString(), out var result))
            return result;

        return null;
    }

    public static bool MaiorQueZero(this object? value) => value is null ? false : value.ToDecimalOrNull() > 0;

    public static string ToStr(this object? value) => value!.ToString() ?? string.Empty;
}
