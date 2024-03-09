namespace ReportService.Core.Abstractions;

public interface IHumanResourcesService
{ 
    public Task<string> GetEmployeeCode(string inn, CancellationToken ct);
}