using ReportService.Core.Abstractions;
using ReportService.Core.Models;

namespace ReportService.Core.Services;

public class EmployeeFactory : IEmployeeFactory
{
    private readonly IAccountingService _accountingService;
    private readonly IHumanResourcesService _humanResourcesService;

    public EmployeeFactory(IAccountingService accountingService, IHumanResourcesService humanResourcesService)
    {
        _accountingService = accountingService;
        _humanResourcesService = humanResourcesService;
    }

    public Employee Create(string name, string inn, Department department)
    {
        return new Employee(name, inn, department, _accountingService, _humanResourcesService);
    }
}