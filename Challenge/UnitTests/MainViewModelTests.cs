using Challenge;
using Challenge.Helpers;
using Challenge.Models;
using Challenge.Service.CurrencyService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Challenge.UnitTests
{
    [TestClass]
    public class MainViewModelTests
    {
        private Mock<ICurrencyService> _currencyServiceMock;
        private MainViewModel _viewModel;
        private readonly CancellationToken cts;

        [TestInitialize]
        public void Setup()
        {
            _currencyServiceMock = new Mock<ICurrencyService>();

            // Setup default for GetAvailableCurrenciesAsync
            _currencyServiceMock.Setup(service => service.GetAvailableCurrenciesAsync(cts))
                .ReturnsAsync(new Dictionary<string, string>
                {
                { "EUR", "Euro" },
                { "USD", "US Dollar" }
                });

            // Setup default for GetLatestRateAsync
            _currencyServiceMock.Setup(service => service.GetLatestRateAsync(It.IsAny<string>(), It.IsAny<string>(), cts))
                .ReturnsAsync(new LatestRateResponse
                {
                    Date = "2025-11-01",
                    Rates = new Dictionary<string, decimal> { { "USD", 1.5m } }
                });

            // Setup default for GetHistoricalRatesAsync
            _currencyServiceMock.Setup(service => service.GetHistoricalRatesAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(), cts))
                .ReturnsAsync(new HistoricalRateResponse
                {
                    Rates = new Dictionary<string, Dictionary<string, decimal>>
                    {
                    { "2025-10-01", new Dictionary<string, decimal> { { "USD", 1.4m } } },
                    { "2025-10-15", new Dictionary<string, decimal> { { "USD", 1.45m } } }
                    }
                });

            // Create ViewModel instance with mocked service
            _viewModel = new MainViewModel(_currencyServiceMock.Object);

            // Wait for async init to complete
            Task.Delay(200).Wait();
        }

        [TestMethod]
        public void Constructor_ShouldInitializeDefaultValues()
        {
            Assert.AreEqual(100m, _viewModel.Amount);
            Assert.Contains("EUR", _viewModel.AvailableCurrencies);
            Assert.Contains("USD", _viewModel.AvailableCurrencies);
            Assert.AreEqual("EUR", _viewModel.SelectedSourceCurrency);
            Assert.AreEqual("USD", _viewModel.SelectedTargetCurrency);
            Assert.IsNotEmpty(_viewModel.HistoricalRates);
        }

        [TestMethod]
        public void UpdateRatesAsync_ShouldUpdateLatestConvertedAmountAndDate()
        {
            Assert.AreEqual(100m * 1.5m, _viewModel.LatestConvertedAmount);
            Assert.AreEqual("2025-11-01", _viewModel.LatestRateDate);
        }


    }
}
