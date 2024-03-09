using ReportService.Core.Dto;
using ReportService.Core.Models;

namespace ReportService.Core.Abstractions;

public interface IEmployeeRepository
{
    Task<IReadOnlyCollection<EmployeeDto>> GetAllEmployees();
}