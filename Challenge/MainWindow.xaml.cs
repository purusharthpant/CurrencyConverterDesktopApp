using Challenge.Service;
using System.Net.Http;
using System.Windows;

namespace Challenge
{
    public partial class MainWindow : Window
    {
        private ConversionController Controller { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ICurrencyRepository repository = new CurrencyRepository();
            ICurrencyService service = new CurrencyService(repository);
            DataContext = new MainViewModel(service);
        }
    }
}
