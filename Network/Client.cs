using System.Net.Sockets;
using System.Net;
using System.Text;
using Network.Data;
using Newtonsoft.Json;

namespace Network
{
    public sealed class Client
    {
        public int isEnd = 0;
        EndPoint remoteEndPoint;
        private int localPort; // local port
        bool appQuit;
        Socket socket;
        int idShip;
        public NetworkData MyCharacter, EnemyCharacter;

        public delegate void ReceiveHandler(string message);
        public event ReceiveHandler Notify;
        public Client(string ip, int remotePort, int localPort, int idShip)
        {
            try
            {
                this.localPort = localPort;
                remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), remotePort);
                appQuit = false;
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint localIP = new IPEndPoint(IPAddress.Any, localPort);
                socket.Bind(localIP);
                this.idShip = idShip;
                RunConnect();
                MyCharacter = new NetworkData();
                EnemyCharacter = new NetworkData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void RunConnect()
        {
            Thread receiveThread = new Thread(new ThreadStart(Connect));
            receiveThread.Start();
        }

        public void Connect()
        {
            Thread.Sleep(100);
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(idShip.ToString());
                socket.SendTo(data, remoteEndPoint);
                ReceiveData();
                ReceiveData();
                RunGameLogic();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ReceiveData()
        {
            try
            {
                byte[] data = new byte[1024 * 8]; // получаем данные
                int bytes = socket.Receive(data);
                string message = Encoding.Unicode.GetString(data, 0, bytes);

                Notify?.Invoke(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void RunGameLogic()
        {
            Thread receiveThread = new Thread(new ThreadStart(ShipDataRecieve));
            receiveThread.Start();
            Thread sendThread = new Thread(new ThreadStart(ShipDataSend));
            sendThread.Start();
        }

        public void ShipDataRecieve()
        {
            IPEndPoint remoteIp = null; // адрес входящего подключения

            while (!appQuit)
            {
                byte[] data = new byte[1024 * 8]; // получаем данные
                int bytes = socket.Receive(data); // получаем данные

                //var result = BitConverter.ToString(data, 0, data.Length);

                // var networkData = JsonConvert.DeserializeObject<NetworkData>(result);

                EnemyCharacter.PlayerPosition = [BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4)];
                //EnemyCharacter.PlayerSpriteFlip = networkData.PlayerSpriteFlip;
                //EnemyCharacter.PrizeId = networkData.PrizeId;
                //EnemyCharacter.PrizePosition = networkData.PrizePosition;
                //EnemyCharacter.SpellTypeId = networkData.SpellTypeId;
                //EnemyCharacter.SpellSpawnPosition = networkData.SpellSpawnPosition;
                //EnemyCharacter.SpellDirection = networkData.SpellDirection;
                //EnemyCharacter.SpellTag = networkData.SpellTag;
                //EnemyCharacter.SpellPower = networkData.SpellPower;
                //isEnd = networkData.IsGameEnd;

                //EnemyShip.x = BitConverter.ToInt32(data, 0);
                //EnemyShip.y = BitConverter.ToInt32(data, 4);
                //EnemyShip.dircetion = BitConverter.ToInt32(data, 8);
                //EnemyShip.bullet = BitConverter.ToInt32(data, 12);
                //EnemyShip.mode = BitConverter.ToInt32(data, 16);
                //isEnd = BitConverter.ToInt32(data, 20);
            }

        }

        public void ShipDataSend()
        {
            try
            {
                while (!appQuit)
                {
                    byte[] data = BitConverter.GetBytes(MyCharacter.PlayerPosition[0])
                        .Concat(BitConverter.GetBytes(MyCharacter.PlayerPosition[1]))
                        .ToArray();

                    socket.SendTo(data, 0, data.Length - 1, SocketFlags.None, remoteEndPoint);
                    Thread.Sleep(30);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CloseClient()
        {
            if (socket != null)
                socket.Close();
            appQuit = true;
        }

        public void ClearNotify()
        {
            Notify = null;
        }
    }
}
