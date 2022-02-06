using System.Text.Json;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System;

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

                    string msg = Encoding.UTF8.GetString(ByteBit(bytes));
                    if (msg == "ok")
                        Minewseeper.minewseeper.baseLog.Add("CLIENT:CONNECTED");
                    else
                        HandleData(msg);
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

        public byte[] ByteBit(byte[] bytes)
        {
            int count = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] != 0)
                    count++;
            }

            byte[] _bytes = new byte[count];
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] != 0)
                    _bytes[i] = bytes[i];
            }

            return _bytes;
        }



        // обработка всех сообщений из сети
        private void HandleData(string msg)
        {
            string[] data = msg.Split('=');
            if (data[0] == "map")
            {
                string[] str = (string[])JsonSerializer.Deserialize(data[1], typeof(string[]));
                for (int i = 0; i < str.Length; i++)
                {
                    string[] s = str[i].Split(':');
                    int w = Convert.ToInt32(s[0]);
                    int h = Convert.ToInt32(s[1]);
                    Minewseeper.minewseeper.map.Field[w, h] = 10;
                }
            }

            if (data[0] == "smap")
            {
                string str = (string)JsonSerializer.Deserialize(data[1], typeof(string));
                string[] s = str.Split(':');
                Minewseeper.minewseeper.map.Generate(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), 10, false);
            }
        }
    }
}
