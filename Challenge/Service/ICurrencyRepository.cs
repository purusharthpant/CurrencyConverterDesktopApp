using Challenge.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Challenge.Service
{
    public interface ICurrencyRepository
    {
        Task<Dictionary<string, string>> GetAvailableCurrenciesAsync(CancellationToken token);
        Task<HistoricalRateResponse> GetHistoricalRatesAsync(string from, string to, DateTime start, DateTime end, CancellationToken token);
        Task<LatestRateResponse> GetLatestRateAsync(string from, string to, CancellationToken token);
    }
}