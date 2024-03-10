using AutoFixture;
using ReportService.Core.Dto;

namespace ReportService.Tests;

public static class SeedDataHelper
{
    public static List<EmployeeDto> GetEmployees()
    {
        var employeeDtos = new Fixture()
            .Build<EmployeeDto>()
            .With(x => x.DepartmentId, "1")
            .With(x => x.DepartmentName, "IT")
            .CreateMany(6)
            .ToList();

        employeeDtos[0].DepartmentId = "2";
        employeeDtos[0].DepartmentName = "Accounting";

        return employeeDtos;
    }
}