using Challenge.Models;
using Challenge.Service;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Challenge
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ICurrencyService _currencyService;

        private ObservableCollection<string> _availableCurrencies;
        private string _selectedSourceCurrency;
        private string _selectedTargetCurrency;
        private decimal _amount;
        private DateTime _startDate;
        private DateTime _endDate;
        private decimal _latestConvertedAmount;
        private string _latestRateDate;
        private ObservableCollection<CurrencyRate> _historicalRates;
        private bool _isLoading;

        public ObservableCollection<string> AvailableCurrencies
        {
            get => _availableCurrencies;
            set => SetProperty(ref _availableCurrencies, value);
        }

        public string SelectedSourceCurrency
        {
            get => _selectedSourceCurrency;
            set
            {
                if (SetProperty(ref _selectedSourceCurrency, value))
                {
                    _ = UpdateRatesAsync();
                }
            }
        }

        public string SelectedTargetCurrency
        {
            get => _selectedTargetCurrency;
            set
            {
                if (SetProperty(ref _selectedTargetCurrency, value))
                {
                    _ = UpdateRatesAsync();
                }
            }
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                if (SetProperty(ref _amount, value))
                {
                    _ = UpdateRatesAsync();
                }
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (SetProperty(ref _startDate, value))
                {
                    _ = UpdateRatesAsync();
                }
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (SetProperty(ref _endDate, value))
                {
                    _ = UpdateRatesAsync();
                }
            }
        }

        public decimal LatestConvertedAmount
        {
            get => _latestConvertedAmount;
            set => SetProperty(ref _latestConvertedAmount, value);
        }

        public string LatestRateDate
        {
            get => _latestRateDate;
            set => SetProperty(ref _latestRateDate, value);
        }

        public ObservableCollection<CurrencyRate> HistoricalRates
        {
            get => _historicalRates;
            set => SetProperty(ref _historicalRates, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public MainViewModel(ICurrencyService currencyService)
        {
            _currencyService = currencyService;

            HistoricalRates = new ObservableCollection<CurrencyRate>();

            // Set default dates
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
            Amount = 100;

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            IsLoading = true;
            try
            {
                var currencies = await _currencyService.GetAvailableCurrenciesAsync();
                AvailableCurrencies = new ObservableCollection<string>(currencies.Keys);

                // Set default currencies
                if (AvailableCurrencies.Any())
                {
                    SelectedSourceCurrency = "EUR";
                    SelectedTargetCurrency = "USD";
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task UpdateRatesAsync()
        {
            if (string.IsNullOrEmpty(SelectedSourceCurrency) ||
                string.IsNullOrEmpty(SelectedTargetCurrency) ||
                Amount <= 0)
                return;

            IsLoading = true;
            try
            {
                // Fetch latest rate
                var latestTask = _currencyService.GetLatestRateAsync(
                    SelectedSourceCurrency,
                    SelectedTargetCurrency);

                // Fetch historical rates
                var historicalTask = _currencyService.GetHistoricalRatesAsync(
                    SelectedSourceCurrency,
                    SelectedTargetCurrency,
                    StartDate,
                    EndDate);

                await Task.WhenAll(latestTask, historicalTask);

                var latestResponse = await latestTask;
                var historicalResponse = await historicalTask;

                // Process latest rate
                if (latestResponse.Rates.TryGetValue(SelectedTargetCurrency, out var rate))
                {
                    LatestConvertedAmount = Amount * rate;
                    LatestRateDate = latestResponse.Date;
                }

                // Process historical rates using LINQ
                var historicalList = historicalResponse.Rates
                    .OrderBy(kvp => kvp.Key)
                    .Select(kvp => new CurrencyRate
                    {
                        Date = DateTime.Parse(kvp.Key),
                        Rate = kvp.Value[SelectedTargetCurrency],
                        Currency = SelectedTargetCurrency,
                        ConvertedAmount = Amount * kvp.Value[SelectedTargetCurrency]
                    })
                    .ToList();

                HistoricalRates.Clear();
                foreach (var item in historicalList)
                {
                    HistoricalRates.Add(item);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Network error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Data parsing error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
