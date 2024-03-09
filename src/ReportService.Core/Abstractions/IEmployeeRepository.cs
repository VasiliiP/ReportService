using ReportService.Core.Dto;

namespace ReportService.Core.Abstractions;

public interface IEmployeeRepository
{
    Task<IReadOnlyCollection<EmployeeDto>> GetAllEmployees(CancellationToken ct);
}