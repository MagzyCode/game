using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkLib
{
    public class Server
    {
        Queue<byte[]> queue = new Queue<byte[]>();
        UdpClient server;
        private int localPort; // local port
        private bool appClose;
        List<IPEndPoint> clients;

        public delegate void ReceiveHandler(string message);
        public event ReceiveHandler Notify;

        public Server(int localPort)
        {
            try
            {
                this.localPort = localPort;
                appClose = false;
                server = new UdpClient(localPort);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Send(IPEndPoint client, string message)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                server.Send(data, data.Length, client);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }

        public void RunServer()
        {
            RunReceive();
        }

        private void RunReceive()
        {
            Thread receiveThread = new Thread(new ThreadStart(ReceiveDataConnect));
            receiveThread.Start();
        }

        public void ReceiveDataConnect()
        {
            clients = new List<IPEndPoint>();
            IPEndPoint remoteIp = null;
            try
            {
                while (!appClose && clients.Count < 2)
                {
                    byte[] data = server.Receive(ref remoteIp); // получаем данные
                    clients.Add(remoteIp);
                    Notify?.Invoke($"{remoteIp.Address}:{remoteIp.Port} - Ready to connect!");
                }
                Notify?.Invoke($"Play!");

                Send(clients[0], "1");
                Send(clients[1], "2");
                RunPlayLogic();
            }
            catch (Exception ex)
            {
                Notify?.Invoke(ex.Message);
            }
        }

        void RunPlayLogic()
        {
            Thread receiveThread = new Thread(new ThreadStart(ReceivePlayLogic));
            receiveThread.Start();
            Thread sendThread = new Thread(new ThreadStart(SendPlayLogic));
            sendThread.Start();
        }

        void ReceivePlayLogic()
        {
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                while (!appClose)
                {
                    byte[] data = server.Receive(ref remoteIp); // получаем данные
                    Array.Resize(ref data, data.Length + 1);
                    if (clients[0].Address.Equals(remoteIp.Address))
                        data[data.Length - 1] = 1;
                    else
                        data[data.Length - 1] = 0;

                    queue.Enqueue(data);
                }
            }
            catch (Exception ex)
            {
                Notify?.Invoke(ex.Message);
            }
        }

        public string[] GetIpAddress()
        {
            string Host = Dns.GetHostName();

            return Dns.GetHostByName(Host).AddressList.Select(x => x.ToString()).ToArray(); //.Where(x => x.ToString().StartsWith("192.168.0.")).LastOrDefault().ToString();
        }

        void SendPlayLogic()
        {
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                byte[] data;
                while (!appClose)
                {
                    if (queue.Count != 0)
                    {
                        data = queue.Dequeue();
                        server.Send(data, data.Length - 1, clients[data[data.Length - 1]]);
                    }
                }
            }
            catch (Exception ex)
            {
                Notify?.Invoke(ex.Message);
            }
        }

        public void Close()
        {
            server?.Close();
            appClose = true;
        }

        public void ClearNotify()
        {
            Notify = null;
        }
    }
}
