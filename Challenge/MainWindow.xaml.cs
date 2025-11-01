using Challenge.Service;
using Challenge.Service.CacheService;
using Challenge.Service.CurrencyService;
using System.Configuration;
using System.Net.Http;
using System.Windows;

namespace Challenge
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ICurrencyRepository repository = new CurrencyRepository(new CacheService());
            ICurrencyService service = new CurrencyService(repository);
            DataContext = new MainViewModel(service);
        }
    }
}
