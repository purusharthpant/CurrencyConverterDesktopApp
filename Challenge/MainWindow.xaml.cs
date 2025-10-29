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
            ICurrencyService service = new CurrencyService();
            DataContext = new MainViewModel(service);
        }
    }
}
