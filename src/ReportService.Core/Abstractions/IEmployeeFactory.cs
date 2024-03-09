using ReportService.Core.Models;

namespace ReportService.Core.Abstractions;

public interface IEmployeeFactory
{
    Employee Create(string name, string inn, Department department);
}