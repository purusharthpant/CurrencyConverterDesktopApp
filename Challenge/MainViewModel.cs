using Challenge.Helpers;
using Challenge.Models;
using Challenge.Service.CurrencyService;
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
        #region Private Fields
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
        private bool _isInitializing;
        #endregion

        #region Properties
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
                    if(!_isInitializing)
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
                    if(!_isInitializing)
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
                    if (!_isInitializing)
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
                    if(!_isInitializing)
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
                    if(!_isInitializing)
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

        #endregion

        #region Constructor
        public MainViewModel(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
            HistoricalRates = new ObservableCollection<CurrencyRate>();

            _ = InitializeAsync();
        }

        private void SetDefaults()
        {
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
            Amount = 100;
        }
        #endregion

        #region Functions
        private async Task InitializeAsync()
        {
            _isInitializing = true;
            try
            {
                SetDefaults();

                var currencies = await _currencyService.GetAvailableCurrenciesAsync();
                AvailableCurrencies = new ObservableCollection<string>(currencies.Keys);

                // Set default currencies
                if (AvailableCurrencies.Any())
                {
                    SelectedSourceCurrency = "EUR";
                    SelectedTargetCurrency = "USD";
                }
            }
            catch (HttpRequestException ex)
            {
                UIHelper.ShowError(ex);
            }
            catch (JsonException ex)
            {
                UIHelper.ShowError(ex);
            }
            catch (Exception ex)
            {
               UIHelper.ShowError(ex);
            }
            finally
            {
                _isInitializing = false;
                await UpdateRatesAsync();
            }
        }

        private async Task UpdateRatesAsync()
        {
            if (Amount < 0)
            {
                UIHelper.ShowInfo("Invalid amount entered, please enter a valid amount");
                return;
            }

            try
            {
                // Fetch latest rate
                var latestRateResponse = await _currencyService.GetLatestRateAsync(
                    SelectedSourceCurrency,
                    SelectedTargetCurrency);

                // Fetch historical rates
                var historicalRateResponse = await _currencyService.GetHistoricalRatesAsync(
                    SelectedSourceCurrency,
                    SelectedTargetCurrency,
                    StartDate,
                    EndDate);

                // Process latest rate
                if (latestRateResponse.Rates.TryGetValue(SelectedTargetCurrency, out var rate))
                {
                    LatestConvertedAmount = Amount * rate;
                    LatestRateDate = latestRateResponse.Date;
                }

                // Process historical rates using LINQ
                var historicalList = historicalRateResponse.Rates
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
                UIHelper.ShowError(ex);
            }
            catch (JsonException ex)
            {
                UIHelper.ShowError(ex);
            }
            catch (Exception ex)
            {
                UIHelper.ShowError(ex);
            }
        }
        #endregion
    }
}
