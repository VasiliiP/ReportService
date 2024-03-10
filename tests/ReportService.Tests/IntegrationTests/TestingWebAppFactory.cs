using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ReportService.Core.Abstractions;

namespace ReportService.Tests.IntegrationTests;

internal class TestingWebAppFactory : WebApplicationFactory<Program>
{
    private readonly Mock<IHumanResourcesService> _hrMock;
    private readonly Mock<IAccountingService> _accountingMock;
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;

    public TestingWebAppFactory(Mock<IHumanResourcesService> hrMock, Mock<IAccountingService> accountingMock, Mock<IEmployeeRepository> employeeRepositoryMock)
    {
        _hrMock = hrMock;
        _accountingMock = accountingMock;
        _employeeRepositoryMock = employeeRepositoryMock;
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ReplaceScopedService(services, _employeeRepositoryMock.Object);
            ReplaceScopedService(services, _accountingMock.Object);
            ReplaceScopedService(services, _hrMock.Object);
        });
    }

    private static void ReplaceScopedService<T>(IServiceCollection services, T instance) where T : class
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
        services.AddScoped<T>(_ => instance);
    }
}
