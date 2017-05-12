using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TcpChatApplication_1
{
    class MyTcpClientWrapper
    {
        public TcpClient clientSocket = null;
        public NetworkStream networkStream = null;

        public void GetClient(TcpClient var)
        {
            clientSocket = var;
        }

        public string GetString()
        {
            networkStream = clientSocket.GetStream();

            byte[] bytesFrom = new byte[10025];
            networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);

            string dataFromClient = Encoding.ASCII.GetString(bytesFrom);
            dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

            return dataFromClient;
        }

        public void PutString(string serverResponse)
        {
            Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
            networkStream.Write(sendBytes, 0, sendBytes.Length);
            networkStream.Flush();

            clientSocket.Close();
        }
    }

    class MyServer
    {
        public TcpListener serverSocket = null;
        public TcpClient clientSocket = null;
        public NetworkStream networkStream = null;

        public void Start(string ip, int port)
        {
            serverSocket = new TcpListener(IPAddress.Parse(ip), port);
            clientSocket = default(TcpClient);
            serverSocket.Start();
        }

        public TcpClient WaitForClient()
        {
            clientSocket = serverSocket.AcceptTcpClient();

            return clientSocket;
        }

        public void Close()
        {
            serverSocket.Stop();
        }
    }

    class Program
    {
        public static MyServer s = new MyServer();
        public static MyTcpClientWrapper c = new MyTcpClientWrapper();

        static void Main(string[] args)
        {
            try
            {
                s.Start("127.0.0.1", 8888);

                c.GetClient(s.WaitForClient());
                Console.WriteLine(c.GetString());
                c.PutString("Done" + "$");
                
                s.Close();
            }
            catch (Exception) { }

            Console.ReadKey();
        }

    }
}
