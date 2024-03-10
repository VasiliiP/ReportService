using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ReportService.Core.Abstractions;
using ReportService.Core.Dto;
using ReportService.Core.Models;
using ReportService.Core.Services;

namespace ReportService.Tests;

public class SalaryReportServiceTests : UnitTestBase<SalaryReportService>
{
    private readonly Mock<IAccountingService> _accountingServiceMock = new();
    private readonly Mock<IHumanResourcesService> _hrServiceMock = new();
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock = new();
    private readonly Mock<IFileService> _fileServiceMock = new();


    public override void Setup()
    {
        var employeeFactory = new EmployeeFactory(_accountingServiceMock.Object, _hrServiceMock.Object);

        Instance = new SalaryReportService(
            _employeeRepositoryMock.Object,
            employeeFactory,
            _fileServiceMock.Object,
            new Mock<ILogger<SalaryReportService>>().Object
        );
        
        _accountingServiceMock.Invocations.Clear();
        _fileServiceMock.Invocations.Clear();
        _hrServiceMock.Invocations.Clear();
        _employeeRepositoryMock.Invocations.Clear();
    }

    [Test]
    public async Task GenerateSalaryReport_ShouldVerify_Behaviour()
    {
        // arrange
        var employees = SeedDataHelper.GetEmployees();
        var employeesCount = employees.Count;

        var expectedDepartmentsAmount = employees
            .DistinctBy(x => new { x.DepartmentId, x.DepartmentName })
            .Count();

        var actualDepartments = new List<Department>();
        _employeeRepositoryMock.Setup(x =>
            x.GetAllEmployees(It.IsAny<CancellationToken>())).ReturnsAsync(employees);

        _fileServiceMock.Setup(x =>
                x.CreateSalaryReport(It.IsAny<List<Department>>(), It.IsAny<string>()))
            .Returns(Array.Empty<byte>())
            .Callback<List<Department>, string>((departments, period) => { actualDepartments = departments; });

        _accountingServiceMock
            .Setup(x => x.GetSalary(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(20000.00m);

        _hrServiceMock
            .Setup(x => x.GetEmployeeCode(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Fixture.Create<string>());

        // act
        var act = () => Instance.GenerateSalaryReport(2024, 2, CancellationToken.None);
        // assert
        await act.Should().NotThrowAsync();
        actualDepartments.Should().HaveCount(expectedDepartmentsAmount);

        _fileServiceMock.Verify(x => x.CreateSalaryReport(It.IsAny<List<Department>>(), It.IsAny<string>()),
            Times.Once);
        
        _employeeRepositoryMock.Verify(x => x.GetAllEmployees(It.IsAny<CancellationToken>()),
            Times.Once);


        _hrServiceMock.Verify(x => x.GetEmployeeCode(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Exactly(employeesCount));
        
        _accountingServiceMock.Verify(x => x.GetSalary(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), 
            Times.Exactly(employeesCount));
    }

    
}