using System.Net.Sockets;
using System.Net;
using System.Text;
using Network.Data;
using System.Threading;
using System;
using System.Linq;

namespace NetworkLib
{
    public sealed class Client
    {
        public int isEnd = 0;
        EndPoint remoteEndPoint;
        private int localPort; // local port
        bool appQuit;
        Socket socket;
        public NetworkData MyCharacter = new NetworkData();
        public NetworkData EnemyCharacter = new NetworkData();

        public delegate void ReceiveHandler(string message);
        public event ReceiveHandler Notify;
        public Client(string ip, int remotePort, int localPort, ReceiveHandler receiveHandler)
        {
            try
            {
                this.localPort = localPort;
                remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), remotePort);
                appQuit = false;
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint localIP = new IPEndPoint(IPAddress.Any, localPort);
                socket.Bind(localIP);
                Notify += receiveHandler;
                RunConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        public string PlayerId { get; set; }

        public void RunConnection()
        {
            Thread receiveThread = new Thread(new ThreadStart(Connect));
            receiveThread.Start();
        }

        public void Connect()
        {
            Thread.Sleep(100);
            try
            {
                var data = new byte[0];// Encoding.Unicode.GetBytes(playerId);
                socket.SendTo(data, remoteEndPoint);
                var response = ReceiveData();
                RunGameLogic();
                Notify?.Invoke(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string ReceiveData()
        {
            try
            {
                byte[] data = new byte[1024 * 8]; // получаем данные
                int bytes = socket.Receive(data);
                string message = Encoding.Unicode.GetString(data, 0, bytes);

                return message;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private void RunGameLogic()
        {
            Thread receiveThread = new Thread(new ThreadStart(PlayerDataRecieve));
            receiveThread.Start();
            Thread sendThread = new Thread(new ThreadStart(PlayerDataSend));
            sendThread.Start();
        }
        
        public void PlayerDataRecieve()
        {
            IPEndPoint remoteIp = null; // адрес входящего подключения

            while (!appQuit)
            {
                byte[] data = new byte[1024 * 8]; // получаем данные
                int bytes = socket.Receive(data); // получаем данные
                 
                if (bytes > 30) 
                {
                    EnemyCharacter.PlayerPosition = new float[] { BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4) };
                    EnemyCharacter.PrizeSpawnPosition = new float[] { BitConverter.ToSingle(data, 8), BitConverter.ToSingle(data, 12) };
                    EnemyCharacter.PrizeSpawnType = BitConverter.ToInt32(data, 16);
                    EnemyCharacter.SpellCount = BitConverter.ToInt32(data, 20);
                    EnemyCharacter.HealthCount = BitConverter.ToInt32(data, 24);
                    EnemyCharacter.CoinCount = BitConverter.ToInt32(data, 28);
                    EnemyCharacter.IsPlayerSpriteFlip = BitConverter.ToBoolean(data, 32);
                    EnemyCharacter.IsPlayerShooting = BitConverter.ToBoolean(data, 33);
                }
            }
        }

        public void PlayerDataSend()
        {
            try
            {
                while (!appQuit)
                {
                    byte[] data = BitConverter.GetBytes(MyCharacter.PlayerPosition[0])
                           .Concat(BitConverter.GetBytes(MyCharacter.PlayerPosition[1]))
                           .Concat(BitConverter.GetBytes(MyCharacter.PrizeSpawnPosition[0]))
                           .Concat(BitConverter.GetBytes(MyCharacter.PrizeSpawnPosition[1]))
                           .Concat(BitConverter.GetBytes(MyCharacter.PrizeSpawnType))
                           .Concat(BitConverter.GetBytes(MyCharacter.SpellCount))
                           .Concat(BitConverter.GetBytes(MyCharacter.HealthCount))
                           .Concat(BitConverter.GetBytes(MyCharacter.CoinCount))
                           .Concat(BitConverter.GetBytes(MyCharacter.IsPlayerSpriteFlip))
                           .Concat(BitConverter.GetBytes(MyCharacter.IsPlayerShooting))
                           .ToArray();

                    socket.SendTo(data, 0, data.Length, SocketFlags.None, remoteEndPoint);

                    MyCharacter.IsPlayerTryGetPrize = false;
                    MyCharacter.IsPlayerShooting = false;
                    MyCharacter.PrizeSpawnPosition = new float[2];
                    MyCharacter.PrizeSpawnType = -1;
                    Thread.Sleep(30);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CloseConnection()
        {
            socket?.Close();
            appQuit = true;
        }

        public void ClearNotifyEvent()
        {
            Notify = null;
        }
    }
}
