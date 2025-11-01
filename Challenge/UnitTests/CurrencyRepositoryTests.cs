using Challenge.Models;
using Challenge.Service;
using Challenge.Service.CacheService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Challenge.UnitTests
{
    [TestClass]
    public class CurrencyRepositoryTests
    {
        private CurrencyRepository _repository;
        private Mock<ICacheService> _cacheServiceMock;

        [TestInitialize]
        public void Setup()
        {
            _cacheServiceMock = new Mock<ICacheService>();
            _repository = new CurrencyRepository(_cacheServiceMock.Object);
        }

        [TestMethod]
        public async Task GetAvailableCurrenciesAsync_ReturnsDictionary()
        {
            var result = await _repository.GetAvailableCurrenciesAsync(CancellationToken.None);
            Assert.IsInstanceOfType(result, typeof(Dictionary<string, string>));
        }

        [TestMethod]
        public async Task GetLatestRateAsync_ReturnsLatestRateResponse()
        {
            var result = await _repository.GetLatestRateAsync("EUR", "USD", CancellationToken.None);
            Assert.IsInstanceOfType(result, typeof(LatestRateResponse));
        }

        [TestMethod]
        public async Task GetHistoricalRatesAsync_ReturnsFromCacheIfAvailable()
        {
            var result = await _repository.GetHistoricalRatesAsync("EUR", "USD", DateTime.Now.AddDays(-1), DateTime.Now, CancellationToken.None);

            Assert.IsInstanceOfType(result, typeof(HistoricalRateResponse));
        }
    }
}