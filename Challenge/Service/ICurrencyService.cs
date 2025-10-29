using Challenge.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Challenge.Service
{
    public interface ICurrencyService
    {
        Task<Dictionary<string, string>> GetAvailableCurrenciesAsync(CancellationToken cancellationToken = default);
        Task<LatestRateResponse> GetLatestRateAsync(string fromCurrency, string toCurrency, CancellationToken cancellationToken = default);
        Task<HistoricalRateResponse> GetHistoricalRatesAsync(string fromCurrency, string toCurrency, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}