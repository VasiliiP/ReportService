using ReportService.Core.Abstractions;
using ReportService.Core.Dto;
using ReportService.Core.Helpers;
using ReportService.Core.Models;

namespace ReportService.Core.Services;

public class ReportService : IReportService
{
    public ReportService(IEmployeeRepository employeeRepository, IEmployeeFactory employeeFactory,
        IFileService fileService)
    {
        _employeeRepository = employeeRepository;
        _employeeFactory = employeeFactory;
        _fileService = fileService;
    }

    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeFactory _employeeFactory;
    private readonly IFileService _fileService;


    public async Task<byte[]> GenerateSalaryReport(int year, int month, CancellationToken ct)
    {
        var employeeDtos = await _employeeRepository.GetAllEmployees(ct);

        var departments = GroupByDepartments(employeeDtos);

        // might worth to implement parallel calls with batches and Task.WhenAll()
        foreach (var employee in departments.SelectMany(department => department.Employees))
        {
            if (ct.IsCancellationRequested) break;
            await employee.ObtainSalary(ct);
        }

        return _fileService.CreateSalaryReport(departments, MonthResolver.GetName(year, month));
    }

    private List<Department> GroupByDepartments(IEnumerable<EmployeeDto> employeeDtos)
    {
        var departments = new List<Department>();

        var groups = employeeDtos.GroupBy(e => new { e.DepartmentName, e.DepartmentId });

        foreach (var group in groups)
        {
            var dep = new Department(group.Key.DepartmentId, group.Key.DepartmentName);
            var employees = group.Select(empDto => _employeeFactory.Create(empDto.Name, empDto.Inn, dep));
            dep.Employees.AddRange(employees);
            departments.Add(dep);
        }

        return departments;
    }
}