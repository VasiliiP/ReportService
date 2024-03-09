using ReportService.Core.Abstractions;

namespace ReportService.Core.Models;

public sealed class Employee
{
    public Employee(string name, string inn, Department department, IAccountingService accountingService,
        IHumanResourcesService humanResources)
    {
        Name = name;
        Department = department;
        Inn = inn;
        _accountingService = accountingService;
        _humanResources = humanResources;
    }

    private readonly IAccountingService _accountingService;
    private readonly IHumanResourcesService _humanResources;
    
    public string Name { get; init; }
    public Department Department { get; init; }
    private string Inn { get; init; }
    public int Salary { get; private set; }
    private string BuhCode { get; set; } = null!;

    public async Task ObtainSalary(CancellationToken ct)
    {
        BuhCode = await _humanResources.GetEmployeeCode(Inn, ct);
        var salary = await _accountingService.GetSalary(Inn, BuhCode, ct);
        
        // I suppose that either salary doesn't have digits after the decimal point or it doesn't matter
        // As there is no claim about precision in issues list I made decision to keep it as  is
        Salary = (int)salary;
    }
}