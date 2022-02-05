using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace MinewseeperCoop
{
    class Client
    {
        private TcpClient client;
        private NetworkStream stream;
        private IPEndPoint endPoint;
        private IPAddress address;
        private int port;
        private Thread handler;

        public int PacketSize { get; private set; } = 1024;

        public Client()
        {
            client = new TcpClient();
        }

        public void Connect(string ip = "127.0.0.1", int port = 255)
        {
            try
            {
                address = IPAddress.Parse(ip);
                this.port = port;
                endPoint = new IPEndPoint(address, this.port);
                client.Connect(endPoint);
                stream = client.GetStream();
                handler = new Thread(new ThreadStart(Handle));
                handler.Start();

                Send("ok");

                Minewseeper.minewseeper.baseLog.Add("CLIENT:CONNECTING");
            }
            catch
            {
                Minewseeper.minewseeper.baseLog.Add("CLIENT:CONNECTING:ERROR");
            }
        }

        public void Disconnect()
        {
            handler.Interrupt();
            stream.Close();
            client.Dispose();

            Minewseeper.minewseeper.baseLog.Add("CLIENT:DISCONNECT");
        }

        private void Handle()
        {
            while (true)
            {
                while (stream.DataAvailable)
                {
                    byte[] bytes = new byte[PacketSize];
                    stream.Read(bytes, 0, bytes.Length);

                    Minewseeper.minewseeper.baseLog.Add("CLIENT:RECEIVE");

                    string msg = Encoding.UTF8.GetString(bytes);
                    if ((msg[0] + msg[1]) == 218)
                        Minewseeper.minewseeper.baseLog.Add("CLIENT:CONNECTED");
                }
                Thread.Sleep(1);
            }
        }

        public void Send(string msg)
        {
            try
            {
                byte[] bytes = new byte[PacketSize];
                bytes = Encoding.UTF8.GetBytes(msg);
                stream.Write(bytes, 0, bytes.Length);

                Minewseeper.minewseeper.baseLog.Add("CLIENT:SEND");
            }
            catch
            {
                Minewseeper.minewseeper.baseLog.Add("CLIENT:SEND:ERROR");
            }
        }
    }
}
