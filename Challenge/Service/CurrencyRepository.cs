using Challenge.Models;
using Challenge.Service.CacheService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Challenge.Service
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ICacheService _cacheService;

        public CurrencyRepository(ICacheService cacheService = null)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.frankfurter.dev/v1/")
            };
            _cacheService = cacheService;
        }

        public async Task<Dictionary<string, string>> GetAvailableCurrenciesAsync(CancellationToken token)
        {
            var response = await _httpClient.GetAsync("currencies", token);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public async Task<LatestRateResponse> GetLatestRateAsync(string from, string to, CancellationToken token)
        {
            var response = await _httpClient.GetAsync($"latest?base={from}&symbols={to}", token);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LatestRateResponse>(json);
        }

        public async Task<HistoricalRateResponse> GetHistoricalRatesAsync(string from, string to, DateTime start, DateTime end, CancellationToken token)
        {
            if (_cacheService != null && _cacheService.TryGetValue(from, to, start, end, out var cachedValue))
            {
                return (HistoricalRateResponse)cachedValue;
            }

            string startStr = start.ToString("yyyy-MM-dd");
            string endStr = end.ToString("yyyy-MM-dd");
            var url = $"{startStr}..{endStr}?base={from}&symbols={to}";
            var response = await _httpClient.GetAsync(url, token);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<HistoricalRateResponse>(json);
            
            if (_cacheService != null)
            {
                _cacheService.Set(from, to, start, end, result);
            }

            return result;
        }
    }
}
