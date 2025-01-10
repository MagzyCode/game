using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameApplication
{
    /// <summary>
    /// Interaction logic for ClientConnectionWindow.xaml
    /// </summary>
    public partial class ClientConnectionWindow : Window
    {
        public ClientConnectionWindow()
        {
            InitializeComponent();
        }

        public static string ValidIpAddress { get; private set; }

        public static int ValidServerPort { get; private set; }

        public static int ValidLocalPort { get; private set; }

        public static bool IsWindowCanceled { get; set; } = true;

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //var floatBytes = BitConverter.GetBytes(0.01f).Concat(BitConverter.GetBytes(-1.23f)).ToArray();
                //var result = BitConverter.ToSingle(floatBytes, 0);
                //var result2 = BitConverter.ToSingle(floatBytes, 4);

                // Валидация IP-адреса
                if (!IsValidIpAddress(IpAddressBox.Text))
                {
                    throw new Exception("Invalid IP Address format. Please use IPv4 format (e.g., 192.168.0.1). ");
                }

                // Валидация порта сервера
                if (!int.TryParse(ServerPortBox.Text, out int serverPort) || serverPort < 1 || serverPort > 9999)
                {
                    throw new Exception("Invalid Server Port. Enter a number between 1 and 9999.");
                }

                // Валидация локального порта
                if (!int.TryParse(LocalPortBox.Text, out int localPort) || localPort < 1 || localPort > 9999)
                {
                    throw new Exception("Invalid Local Port. Enter a number between 1 and 9999.");
                }

                // Если валидация прошла успешно, сохраняем значения
                ValidIpAddress = IpAddressBox.Text;
                ValidServerPort = serverPort;
                ValidLocalPort = localPort;

                // Закрытие окна после успешной валидации
                //MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                IsWindowCanceled = false;
                this.DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрытие окна без сохранения данных
            IsWindowCanceled = true;
            this.DialogResult = false;
            Close();
        }

        private bool IsValidIpAddress(string ipAddress)
        {
            // Проверка корректности формата IPv4-адреса
            return IPAddress.TryParse(ipAddress, out IPAddress address) && address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
        }
    }
}
