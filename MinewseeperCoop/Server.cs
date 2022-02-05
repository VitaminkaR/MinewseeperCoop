using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MinewseeperCoop
{
    class Server
    {
        private TcpListener server;
        private IPEndPoint endPoint;
        private IPAddress address;
        private int port;
        private List<NetworkStream> clients;
        private Thread serverLoop;

        public bool IsClose { get; private set; }
        public int PacketSize { get; private set; } = 1024;

        public Server(string ip = "127.0.0.1", int port = 255)
        {
            try
            {
                address = IPAddress.Parse(ip);
                this.port = port;
                IsClose = true;
                endPoint = new IPEndPoint(address, this.port);
                server = new TcpListener(endPoint);
                clients = new List<NetworkStream>();

                Minewseeper.minewseeper.host = true;
                Minewseeper.minewseeper.baseLog.Add("SERVER:CREATE");
            }
            catch
            {
                Minewseeper.minewseeper.baseLog.Add("SERVER:CREATE:ERROR:ALREADY_CREATED");
            }
        }

        public void StartServer()
        {
            try
            {
                server.Start();
                server.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), server);
                IsClose = false;

                serverLoop = new Thread(new ThreadStart(ServerLoop));
                serverLoop.Start();

                Minewseeper.minewseeper.baseLog.Add("SERVER:START");
            }
            catch
            {
                Minewseeper.minewseeper.baseLog.Add("SERVER:START:ERROR");
            }
        }

        public void Stop()
        {
            server.Stop();
            serverLoop.Interrupt();
            server.Server.Close();
            server.Server.Dispose();
            IsClose = true;

            Minewseeper.minewseeper.baseLog.Add("SERVER:STOP");
        }

        private void AcceptClient(IAsyncResult result)
        {
            try
            {
                TcpListener _server = (TcpListener)result.AsyncState;
                clients.Add(_server.EndAcceptTcpClient(result).GetStream());
                Minewseeper.minewseeper.baseLog.Add("SERVER:ACCEPT_CLIENT");
                server.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), server);
            }
            catch
            {
                Minewseeper.minewseeper.baseLog.Add("SERVER:ACCEPT_CLIENT:ERROR");
            }
        }

        private void ServerLoop()
        {
            while (true)
            {
                if (IsClose)
                    break;

                for (int i = 0; i < clients.Count; i++)
                {
                    while (clients[i].DataAvailable)
                    {
                        byte[] bytes = new byte[PacketSize];
                        clients[i].Read(bytes, 0, bytes.Length);

                        // проверка присоединился ли пользователь
                        string msg = Encoding.UTF8.GetString(bytes);
                        if ((msg[0] + msg[1]) == 218)
                        {
                            clients[i].Write(bytes, 0, bytes.Length);
                        }
                        else
                        {
                            Minewseeper.minewseeper.baseLog.Add("SERVER:RECEIVE");
                            SendPacket(bytes, i);
                        }
                    }
                }
                if (!IsClose)
                    Thread.Sleep(1);
            }
        }

        private void SendPacket(byte[] packet, int id)
        {
            try
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    if (i != id)
                        clients[i].Write(packet, 0, packet.Length);
                }
            }
            catch
            {
                Minewseeper.minewseeper.baseLog.Add("SERVER:SEND:ERROR");
            }
        }
    }
}
