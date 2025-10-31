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

            var cacheStrategy = ConfigurationManager.AppSettings["CacheStrategy"] ?? "time";
            var cacheMaxAge = int.Parse(ConfigurationManager.AppSettings["CacheMaxAge"] ?? "60");
            var cacheMaxElements = int.Parse(ConfigurationManager.AppSettings["CacheMaxElements"] ?? "100");

            ICurrencyRepository repository = new CurrencyRepository(new CacheService(cacheMaxAge, cacheMaxElements, cacheStrategy));
            ICurrencyService service = new CurrencyService(repository);
            DataContext = new MainViewModel(service);
        }
    }
}
