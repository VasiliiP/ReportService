namespace ReportService.Core.Abstractions;

public interface IReportService
{
    Task<byte[]> GenerateSalaryReport(int year, int month, CancellationToken ct);
}