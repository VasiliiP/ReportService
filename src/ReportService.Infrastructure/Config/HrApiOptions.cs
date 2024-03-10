namespace ReportService.Infrastructure.Config;

public class HrApiOptions
{
    public string BaseAddress { get; set; } = null!;
    public int CacheTimeToLiveInHours { get; set; }
}