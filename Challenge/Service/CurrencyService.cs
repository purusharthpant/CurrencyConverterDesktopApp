using Challenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Challenge.Service
{
    public class CurrencyService : ICurrencyService
    {
        private static HttpClient _httpClient;
        private const string BASE_URL = "https://api.frankfurter.dev/v1/";

        public CurrencyService()
        {
            _httpClient.BaseAddress = new Uri(BASE_URL);
        }

        public async Task<LatestRateResponse> GetLatestRateAsync(string fromCurrency, string toCurrency)
        {
            var url = $"latest?base={fromCurrency}&symbols={toCurrency}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LatestRateResponse>(json);
        }

        public async Task<HistoricalRateResponse> GetHistoricalRatesAsync(string fromCurrency, string toCurrency, DateTime startDate, DateTime endDate)
        {
            var start = startDate.ToString("yyyy-MM-dd");
            var end = endDate.ToString("yyyy-MM-dd");
            var url = $"{start}..{end}?base={fromCurrency}&symbols={toCurrency}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HistoricalRateResponse>(json);
        }

        public async Task<Dictionary<string, string>> GetAvailableCurrenciesAsync()
        {
            var response = await _httpClient.GetAsync("currencies");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

    }
}
