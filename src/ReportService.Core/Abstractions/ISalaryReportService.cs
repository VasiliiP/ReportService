namespace ReportService.Core.Abstractions;

public interface ISalaryReportService
{
    Task<byte[]> GenerateSalaryReport(int year, int month, CancellationToken ct);
}