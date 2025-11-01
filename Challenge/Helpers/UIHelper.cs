using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace Challenge.Helpers
{
    public static class UIHelper
    {
        static UIHelper()
        {
        }

        public static void ShowError(Exception ex)
        {
            switch (ex)
            {
                case HttpRequestException httpReqEx:
                    MessageBox.Show($"Network error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case JsonException jsonEx:
                    MessageBox.Show($"Data parsing error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                default:
                    MessageBox.Show($"Unexpected error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        public static void ShowInfo(string message)
        {
            // Implementation for showing informational messages to the user
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
