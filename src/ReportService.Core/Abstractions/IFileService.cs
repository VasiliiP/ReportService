using ReportService.Core.Models;

namespace ReportService.Core.Abstractions;

public interface IFileService
{
    byte[] CreateSalaryReport(List<Department> departments, string period);
}