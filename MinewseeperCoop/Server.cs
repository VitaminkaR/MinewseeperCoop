using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;

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
                server.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), null);
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
        }

        private void AcceptClient(IAsyncResult result)
        {
            try
            {
                clients.Add(server.EndAcceptTcpClient(result).GetStream());
                Minewseeper.minewseeper.baseLog.Add("SERVER:ACCEPT_CLIENT");
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
                        Minewseeper.minewseeper.baseLog.Add("SERVER:RECEIVE");
                        SendPacket(bytes);
                        Minewseeper.minewseeper.baseLog.Add("SERVER:SEND");
                    }
                }
                Thread.Sleep(1);
            }
        }

        private void SendPacket(byte[] packet)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Write(packet, 0, packet.Length);
            }
        }
    }
}
