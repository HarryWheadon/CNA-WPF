using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace ServerProj
{
    internal class Server
    {
        private TcpListener m_TcpListener;

        public Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            m_TcpListener = new TcpListener(ip, port);
        }


        public void Start()
        {
            m_TcpListener.Start();
            Console.WriteLine("Listening...");

            Socket socket = m_TcpListener.AcceptSocket();
            Console.WriteLine("Connection Made");
            ClientMethod(socket);
        }
        public void Stop()
        {
            m_TcpListener.Stop();
        }

        private void ClientMethod(Socket socket)
        {
            string receivedMessage;

            NetworkStream stream = new NetworkStream(socket, true);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

            writer.WriteLine("You have connected to the server - send 0 for valid options");
            writer.Flush();

            while((receivedMessage = reader.ReadLine()) != null)
            {
                writer.WriteLine(GetReturnMessage(receivedMessage));
                writer.Flush();

                if(receivedMessage == "end")
                {
                    break;
                }
            }
            socket.Close();
        }

        private string GetReturnMessage(string code) 
        {
            switch(code)
            {
                case "0":
                    return ("you have chosen 0");
                case "1":
                    return ("you have chosen 1");
                case "2":
                    return ("you have chosen 2");
                case "3":
                    return ("you have chosen 3");
                case "4":
                    return ("you have chosen 4");
                case "5":
                    return ("you have chosen 5");
                case "6":
                    return ("you have chosen 6");
                case "7":
                    return ("you have chosen 7");
                case "8":
                    return ("you have chosen 8");
                case "9":
                    return ("you have chosen 9");
                case "10":
                    return ("you have chosen 10");
            }
            return "hello";
        }
    }
}
