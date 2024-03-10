using LazyCache;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReportService.Core.Abstractions;
using ReportService.Infrastructure.Config;

namespace ReportService.Infrastructure.HttpServices;

public class HrCachedService : IHumanResourcesService
{
    private readonly HttpClient _httpClient;
    private readonly IAppCache _cache;
    private readonly ILogger<HrCachedService> _logger;
    private readonly int _timeToLiveInHours;

    public HrCachedService(HttpClient httpClient, IOptions<HrApiOptions> options, IAppCache cache, ILogger<HrCachedService> logger)
    {
        ArgumentException.ThrowIfNullOrEmpty(options.Value.BaseAddress);
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
        _timeToLiveInHours = options.Value.CacheTimeToLiveInHours;
        _httpClient.BaseAddress = new Uri(options.Value.BaseAddress);
    }
    public async Task<string> GetEmployeeCode(string inn, CancellationToken ct)
    {
        return await _cache.GetOrAddAsync(inn, AddItemFactory,TimeSpan.FromHours(_timeToLiveInHours));
        
        async Task<string> AddItemFactory()
        {
            _logger.LogInformation("cache miss for inn: {Inn}", inn);
            return await _httpClient.GetStringAsync($"inn/{inn}", ct);
        }
    }
}