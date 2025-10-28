using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Challenge;
using System;
using System.Threading.Tasks;
using Microsoft.VSDiagnostics;

[SimpleJob(RuntimeMoniker.Net48)]
[CPUUsageDiagnoser]
public class CurrencyConversionBenchmark
{
    private MainViewModel _viewModel;
    [GlobalSetup]
    public async Task Setup()
    {
        _viewModel = new MainViewModel();
        await Task.Delay(2000); // Wait for initialization
        // Set test values
        _viewModel.Amount = 100;
        _viewModel.SelectedSourceCurrency = "EUR";
        _viewModel.SelectedTargetCurrency = "USD";
        _viewModel.StartDate = DateTime.Now.AddMonths(-1);
        _viewModel.EndDate = DateTime.Now;
    }

    [Benchmark]
    public async Task UpdateRates()
    {
        await _viewModel.UpdateRatesAsync();
    }
}