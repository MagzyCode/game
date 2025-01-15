﻿using System.Net.Sockets;
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
                 
                if (bytes == 22) 
                {
                    EnemyCharacter.PlayerPosition = new float[] { BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4) };
                    EnemyCharacter.SpellCount = BitConverter.ToInt32(data, 8);
                    EnemyCharacter.HealthCount = BitConverter.ToInt32(data, 12);
                    EnemyCharacter.CoinCount = BitConverter.ToInt32(data, 16);
                    EnemyCharacter.IsPlayerSpriteFlip = BitConverter.ToBoolean(data, 20);
                    EnemyCharacter.IsPlayerShooting = BitConverter.ToBoolean(data, 21);
                }
            }
        }

        public void PlayerDataSend()
        {
            try
            {
                while (!appQuit)
                {
                    //if (MyCharacter.PlayerPosition[0] != 0.0f && MyCharacter.PlayerPosition[1] != 0.0f)
                    //{
                    //    byte[] data = BitConverter.GetBytes(MyCharacter.PlayerPosition[0])
                    //       .Concat(BitConverter.GetBytes(MyCharacter.PlayerPosition[1]))
                    //       .Concat(BitConverter.GetBytes(MyCharacter.BulletCount))
                    //       .Concat(BitConverter.GetBytes(MyCharacter.HealthCount))
                    //       .Concat(BitConverter.GetBytes(MyCharacter.PlayerId))
                    //       .Concat(BitConverter.GetBytes(MyCharacter.IsPlayerSpriteFlip))
                    //       .Concat(BitConverter.GetBytes(MyCharacter.IsPlayerShooting))
                    //       .ToArray();

                    //    socket.SendTo(data, 0, data.Length, SocketFlags.None, remoteEndPoint);

                    //    MyCharacter.IsPlayerShooting = false;
                    //}
                    byte[] data = BitConverter.GetBytes(MyCharacter.PlayerPosition[0])
                           .Concat(BitConverter.GetBytes(MyCharacter.PlayerPosition[1]))
                           .Concat(BitConverter.GetBytes(MyCharacter.SpellCount))
                           .Concat(BitConverter.GetBytes(MyCharacter.HealthCount))
                           .Concat(BitConverter.GetBytes(MyCharacter.CoinCount))
                           .Concat(BitConverter.GetBytes(MyCharacter.IsPlayerSpriteFlip))
                           .Concat(BitConverter.GetBytes(MyCharacter.IsPlayerShooting))
                           .ToArray();

                    socket.SendTo(data, 0, data.Length, SocketFlags.None, remoteEndPoint);

                    MyCharacter.IsPlayerShooting = false;
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
