using NetworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameServerUdp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int PORT = 5555;
        private bool isServerRunning;
        Action<string> action;
        Server server;
        List<string> consoleText;

        public MainWindow()
        {
            InitializeComponent();
            isServerRunning = false;
            action = ShowMessage;
            consoleText = new List<string>();
        }

        private void RunServerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isServerRunning)
            {
                server = new Server(PORT);
                server.Notify += NotifyDispatcher;
                server.RunServer();
                // var u = server.GetIpAddress();
                // MessageBox.Show(string.Join(", ", u));
                // IpAddressLabel.Content = $"{server.GetIpAddress().First(x => x.StartsWith("192.168.0."))}:{PORT}";
                var possibleIpAddresses = server.GetIpAddress();
                var serverIpAddress = possibleIpAddresses.FirstOrDefault(x => x.StartsWith("192.168.0"))
                    ?? possibleIpAddresses.FirstOrDefault(x => x.StartsWith("192.168."));
                IpAddressLabel.Content = $"{serverIpAddress}:{PORT}";
                consoleText.Clear();
                ShowMessage("Server is running!");
                ShowMessage("Waiting for connections...");
                isServerRunning = true;
                RunServerButton.Content = "Close server";
            }
            else
            {
                server.ClearNotify();
                server.Close();
                server = null;
                consoleText.Clear();
                ShowMessage("Server closed");
                IpAddressLabel.Content = "";
                isServerRunning = false;
                RunServerButton.Content = "Run server";
            }
        }

        private void NotifyDispatcher(string message)
        {
            Dispatcher.Invoke(action, message);
        }

        private void ShowMessage(string message)
        {
            if (consoleText.Count > 10)
                consoleText.RemoveAt(0);
            consoleText.Add(DateTime.Now.ToString() + ": " + message + "\n");
            StringBuilder builder = new StringBuilder();
            foreach (string str in consoleText)
            {
                builder.Append(str);
            }
            ConsoleTextBlock.Text = builder.ToString();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (server != null)
            {
                server.Close();
            }
        }
    }
}
