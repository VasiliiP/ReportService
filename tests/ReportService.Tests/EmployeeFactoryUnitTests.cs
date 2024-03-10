using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using ReportService.Core.Abstractions;
using ReportService.Core.Models;
using ReportService.Core.Services;

namespace ReportService.Tests;

public class EmployeeFactoryUnitTests : UnitTestBase<EmployeeFactory>
{
    private readonly Mock<IAccountingService> _accountingServiceMock = new();
    private readonly Mock<IHumanResourcesService> _hrServiceMock = new();
    
    
    public override void Setup()
    {
        Instance = new EmployeeFactory(_accountingServiceMock.Object, _hrServiceMock.Object);
    }

    [Test, AutoData]
    public void Create_ShouldReturn_Expected(string name, string inn, Department department)
    {
        // arrange
        var expected = new Employee(name, inn, department, _accountingServiceMock.Object, _hrServiceMock.Object);
        // act
        var act = Instance.Create(name, inn, department);
        // assert
        act.Should().BeEquivalentTo(expected);
    }
}