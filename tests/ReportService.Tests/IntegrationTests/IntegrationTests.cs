using System.Net;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using ReportService.Core.Abstractions;

namespace ReportService.Tests.IntegrationTests;

public class IntegrationTests
{
    #region Setup

#pragma warning disable NUnit1032
    private WebApplicationFactory<Program> _webAppFactory = null!;
#pragma warning restore NUnit1032
    private HttpClient Client;
    

    [OneTimeSetUp]
    public virtual void FixtureSetup()
    {
        _webAppFactory = new TestingWebAppFactory(_hrMock, _accountingMock, _employeeRepositoryMock);
    }

    [SetUp]
    public virtual void TestSetup() => Client = _webAppFactory.CreateClient();

    [TearDown]
    public virtual void TestTearDown() => Client.Dispose();

    [OneTimeTearDown]
    public virtual async Task FixtureTearDown()
    {
        if (_webAppFactory == null)
            return;
        IAsyncDisposable webAppFactory = _webAppFactory;
        await webAppFactory.DisposeAsync();
    }

    #endregion
    
    private readonly Mock<IHumanResourcesService> _hrMock = new();
    private readonly Mock<IAccountingService> _accountingMock= new();
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock= new();
    private readonly Fixture _fixture = new();

    [Test]
    public async Task GetReport_ValidRequest_ReturnSuccess()
    {
        // arrange
        var uri = new Uri("http://localhost/report/2024/2");
        
        var employees = SeedDataHelper.GetEmployees();
        
        _employeeRepositoryMock.Setup(x =>
            x.GetAllEmployees(It.IsAny<CancellationToken>())).ReturnsAsync(employees);
        
        _accountingMock
            .Setup(x => x.GetSalary(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(20000.00m);

        _hrMock
            .Setup(x => x.GetEmployeeCode(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Create<string>());

        
        // act
        var response = await Client.GetAsync(uri);
        var responseBody = await response.Content.ReadAsStringAsync();
        var act = () => response.EnsureSuccessStatusCode();
        // assert
        act.Should().NotThrow();
        responseBody.Should().Contain("февраль 2024");
        responseBody.Should().Contain("Accounting");
        responseBody.Should().Contain("IT");
        responseBody.Should().Contain("Всего по предприятию:120000");
    }
    
    [Test]
    public async Task GetReport_InValidRequest_ReturnValidationError()
    {
        // arrange
        var uri = new Uri("http://localhost/report/2024/22");
        
        // act
        var response = await Client.GetAsync(uri);
        var responseBody = await response.Content.ReadAsStringAsync();
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseBody.Should().Contain("Validation error");
        responseBody.Should().Contain("Month: 'Month' must be less than or equal to '12'");
    }
}