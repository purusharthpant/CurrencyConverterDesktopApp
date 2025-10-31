using Challenge.Models;
using Challenge.Service.CurrencyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Challenge.Service
{
    public abstract class CurrencyServiceDecorater : ICurrencyService
    {
        public Task<Dictionary<string, string>> GetAvailableCurrenciesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<HistoricalRateResponse> GetHistoricalRatesAsync(string fromCurrency, string toCurrency, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<LatestRateResponse> GetLatestRateAsync(string fromCurrency, string toCurrency, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
