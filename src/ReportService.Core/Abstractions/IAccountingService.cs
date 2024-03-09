namespace ReportService.Core.Abstractions;

public interface IAccountingService
{
    Task<decimal> GetSalary(string inn, string employeeCode);
}