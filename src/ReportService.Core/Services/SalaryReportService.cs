using Microsoft.Extensions.Logging;
using ReportService.Core.Abstractions;
using ReportService.Core.Dto;
using ReportService.Core.Helpers;
using ReportService.Core.Models;

namespace ReportService.Core.Services;

public class SalaryReportService : ISalaryReportService
{
    public SalaryReportService(IEmployeeRepository employeeRepository, IEmployeeFactory employeeFactory,
        IFileService fileService, ILogger<SalaryReportService> logger)
    {
        _employeeRepository = employeeRepository;
        _employeeFactory = employeeFactory;
        _fileService = fileService;
        _logger = logger;
    }

    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeFactory _employeeFactory;
    private readonly IFileService _fileService;
    private readonly ILogger<SalaryReportService> _logger;


    public async Task<byte[]> GenerateSalaryReport(int year, int month, CancellationToken ct)
    {
        var employeeDtos = await _employeeRepository.GetAllEmployees(ct);
        _logger.LogDebug("{Count} employees were fetched", employeeDtos.Count);

        var departments = GroupByDepartments(employeeDtos);
        _logger.LogDebug("{Count} departments were grouped", departments.Count);


        // might worth to implement parallel calls with batches and Task.WhenAll()
        foreach (var employee in departments.SelectMany(department => department.Employees))
        {
            if (ct.IsCancellationRequested) break;

            try
            {
                await employee.ObtainSalary(ct);
            }
            catch (Exception e)
            {
                _logger.LogError("Error occurs during employee: {Name} salary processing with message: {Msg}", employee.Name, e.Message );
                throw;
            }
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