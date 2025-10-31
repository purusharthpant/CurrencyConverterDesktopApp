using Challenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Challenge.Service.CurrencyService
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public Task<Dictionary<string, string>> GetAvailableCurrenciesAsync(CancellationToken token)
        => _currencyRepository.GetAvailableCurrenciesAsync(token);

        public Task<LatestRateResponse> GetLatestRateAsync(string from, string to, CancellationToken token)
            => _currencyRepository.GetLatestRateAsync(from, to, token);

        public Task<HistoricalRateResponse> GetHistoricalRatesAsync(string from, string to, DateTime start, DateTime end, CancellationToken token)
            => _currencyRepository.GetHistoricalRatesAsync(from, to, start, end, token);
    }
}
