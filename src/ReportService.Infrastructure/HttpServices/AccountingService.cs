using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using ReportService.Core.Abstractions;
using ReportService.Infrastructure.Config;

namespace ReportService.Infrastructure.HttpServices;

public class AccountingService : IAccountingService
{
    private readonly HttpClient _httpClient;

    public AccountingService(HttpClient httpClient, IOptions<AccountingApiOptions> options)
    {
        ArgumentException.ThrowIfNullOrEmpty(options.Value.BaseAddress);
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(options.Value.BaseAddress);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<decimal> GetSalary(string inn, string employeeCode, CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync($"empcode/{inn}", employeeCode, ct);
        response.EnsureSuccessStatusCode();
        var jsonString = await response.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<decimal>(jsonString);
    }
}