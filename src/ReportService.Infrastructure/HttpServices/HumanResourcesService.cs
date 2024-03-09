using Microsoft.Extensions.Options;
using ReportService.Core.Abstractions;
using ReportService.Infrastructure.Config;

namespace ReportService.Infrastructure.HttpServices;

public class HumanResourcesService : IHumanResourcesService
{
    private readonly HttpClient _httpClient;

    public HumanResourcesService(HttpClient httpClient, IOptions<HrApiOptions> options)
    {
        ArgumentException.ThrowIfNullOrEmpty(options.Value.BaseAddress);
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(options.Value.BaseAddress);
    }
    public async Task<string> GetEmployeeCode(string inn, CancellationToken ct)
    {
        return await _httpClient.GetStringAsync($"inn/{inn}", ct);
    }
}