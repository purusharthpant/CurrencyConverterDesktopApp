using System.Windows;

namespace Challenge
{
    public partial class MainWindow : Window
    {
        private ConversionController Controller { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
