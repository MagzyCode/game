using EngineLibrary.EngineComponents;
using GameLibrary.Game;
using GameLibrary.Maze;
using NetworkLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameApplication
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private readonly RenderingApplication application;
        private readonly MazeScene mazeScene;
        // private Client _client;
        private string _serverIpAddress;
        private int _serverPort;
        private int _localPort;
        private Action<string> _action;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public GameWindow()
        {
            InitializeComponent();
            _action = RunGame;
            application = new RenderingApplication();
            mazeScene = new MazeScene();
            GameEvents.ChangeCoins += ChangeCoins;
            GameEvents.ChangeHealth += ChangeHealth;
            GameEvents.EndGame += EndGame;
            GameEvents.ChangeEffect += ChangeEffect;
            GameEvents.ChangeCount += ChangeCout;
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(application != null)
                application.Dispose();
        }

        public void OpenGameDispatcher(string message)
        {
            Dispatcher.Invoke(_action, message);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var connectionWindow = new ClientConnectionWindow();
            connectionWindow.ShowDialog();

            if (!ClientConnectionWindow.IsWindowCanceled)
            {
                _serverIpAddress = ClientConnectionWindow.ValidIpAddress;
                _serverPort = ClientConnectionWindow.ValidServerPort;
                _localPort = ClientConnectionWindow.ValidLocalPort;
                mazeScene.Client = new Client(_serverIpAddress, _serverPort, _localPort, OpenGameDispatcher);
            }  
        }

        private void RunGame(string message)
        {
            if (int.TryParse(message, out int _) && mazeScene != null)
            {
                mazeScene.PlayerId = message;
                mazeScene.Client.PlayerId = message;

                mazeScene.Client.ClearNotifyEvent();

                ButtonPlay.IsEnabled = false;

                HelpPanel.Visibility = Visibility.Hidden;
                MenuPanel.Visibility = Visibility.Hidden;

                BluePlayerPanel.Visibility = Visibility.Visible;
                RedPlayerPanel.Visibility = Visibility.Visible;

                formhost.Child = application.RenderForm;

                application.SetScene(mazeScene);
                application.Run();
            }
        }

        private void ButtonRunServer_Click(object sender, RoutedEventArgs e)
        {
            var path = typeof(GameServerUdp.App).Assembly.Location;

            Process.Start(path);
        }

        private void EndGame(string winPlayer)
        {
            formhost.Visibility = Visibility.Hidden;
            string wizard;

            BPEffectText.Text = "";
            RPEffectText.Text = "";

            if (winPlayer == "Blue Player")
            {
                wizard = "Blue Wizard";
                WinPlayerText.Foreground = new SolidColorBrush(Color.FromRgb(9, 46, 168));
            }
            else if (winPlayer == "Red Player")
            {
                wizard = "Red Wizard";
                WinPlayerText.Foreground = new SolidColorBrush(Color.FromRgb(179, 22, 22));
            }
            else
            {
                wizard = "Friendship";
                WinPlayerText.Foreground = new SolidColorBrush(Color.FromRgb(255, 215, 0));
            }
               
            WinPlayerText.Text = wizard + " Wins!";

            Uri resourceLocater = new Uri("/Images/"+ winPlayer + ".png", UriKind.Relative);
            BitmapImage bitmap = new BitmapImage(resourceLocater);

            WinPlayerImage.Source = bitmap;

            WinPanel.Visibility = Visibility.Visible;

            GameEvents.EndGame -= EndGame;
            GameEvents.ChangeCoins -= ChangeCoins;
            GameEvents.ChangeHealth -= ChangeHealth;
        }

        private void ChangeCoins(string player, int value)
        {

            if (player == "Blue Player")
            {
                value += Int32.Parse(BluePlayerCoins.Text);
                BluePlayerCoins.Text = value.ToString();
            }
            else
            {
                value += Int32.Parse(RedPlayerCoins.Text);
                RedPlayerCoins.Text = value.ToString();
            }
        }

        private void ChangeHealth(string player, int value)
        {
            if (player == "Blue Player")
                BluePlayerHealth.Text = value.ToString();
            else
                RedPlayerHealth.Text = value.ToString();
        }

        private void ChangeEffect(string player, string effect)
        {
            TextBlock textBlock;

            if (player == "Blue Player")
                textBlock = BPEffectText;
            else if (player == "Red Player")
                textBlock = RPEffectText;
            else
                textBlock = MonsterEffectText;

            switch (effect)
            {
                case "Death":
                    textBlock.Foreground = new SolidColorBrush(Color.FromRgb(220, 220, 220));
                    break;
                case "Speed":
                    textBlock.Foreground = new SolidColorBrush(Color.FromRgb(65, 105, 225));
                    break;
                case "Power":
                    textBlock.Foreground = new SolidColorBrush(Color.FromRgb(255, 20, 147));
                    break;
                case "Reload":
                    textBlock.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    break;
                case " ":
                    textBlock.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    break;
            }

            textBlock.Text = effect;
        }

        private void ChangeCout(string player, int count)
        {
            if (player == "Blue Player")
                BPCountSpells.Text = count.ToString();
            else
                RPCountSpells.Text = count.ToString();
        }
    }
}
