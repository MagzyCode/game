using Network;
using System.Text;
using System.Windows;

namespace GameServer
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
                IpAddressLabel.Content = $"{server.GetIpAddress()}:{PORT}";
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