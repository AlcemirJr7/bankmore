namespace Core.Extensions;

public static class DateTimeExtensions
{
    public static DateTime Br(this DateTime dateTime) =>
        DateTime.SpecifyKind(dateTime.ToLocalTime(), DateTimeKind.Local).AddHours(-3); // Brasil é UTC-3

    public static string BrStr(this DateTime dateTime, string format = "dd/MM/yyyy HH:mm:ss") =>
        dateTime.Br().ToString(format);
}
