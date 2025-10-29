using Challenge.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge.Service
{
    public interface ICurrencyService
    {
        Task<Dictionary<string, string>> GetAvailableCurrenciesAsync();
        Task<HistoricalRateResponse> GetHistoricalRatesAsync(string fromCurrency, string toCurrency, DateTime startDate, DateTime endDate);
        Task<LatestRateResponse> GetLatestRateAsync(string fromCurrency, string toCurrency);
    }
}