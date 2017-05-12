using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace TcpConnection_Client_1
{
    class MyClient
    {
        public string SendMessage(string ip, int port, string msg)
        {
            TcpClient clientSocket = new TcpClient();
            clientSocket.Connect(ip, port);

            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = Encoding.ASCII.GetBytes(msg + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[10025];
            serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
            string returndata = Encoding.ASCII.GetString(inStream);

            return returndata;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyClient m = new MyClient();
            try
            {
                Console.WriteLine(m.SendMessage("127.0.0.1", 8888, "Hello"));
            }
            catch (Exception) { }

            Console.ReadKey();
        }
    }
}
