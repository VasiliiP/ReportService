using System.Globalization;

namespace ReportService.Core.Helpers;

public static class MonthResolver
{
    /// <summary>
    /// New implementation of MonthResolver instead of version with 'Mihalych pridurok' :)
    /// </summary>
    public static string GetName(int year, int month)
    {
        return new DateTime(year, month, 1).ToString("MMMM yyyy", new CultureInfo("ru-RU"));
    }
}