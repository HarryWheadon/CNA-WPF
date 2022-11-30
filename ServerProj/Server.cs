using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using Packets;

namespace ServerProj
{
    internal class Server
    {
        private TcpListener m_TcpListener;
        private ConcurrentDictionary<int, ConnectedClient> m_clients;

        public Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            m_TcpListener = new TcpListener(ip, port);
        }

        public void Start()
        {
            m_clients = new ConcurrentDictionary<int, ConnectedClient>();
            int clientIndex = 0;

            m_TcpListener.Start();
            Console.WriteLine("Listening...");

            try
            {
                while (true)
                {
                    Socket socket = m_TcpListener.AcceptSocket();
                    Console.WriteLine("Connection Made");

                    ConnectedClient client = new ConnectedClient(socket);
                    int index = clientIndex;
                    clientIndex++;

                    m_clients.TryAdd(index, client);

                    Thread thread = new Thread(() => { ClientMethod(index); });
                    thread.Start();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
        public void Stop()
        {
            m_TcpListener.Stop();
        }

        private void ClientMethod(int index)
        {
            string receivedMessage;

           //m_clients[index].Send("You have connected to the server - send 0 for valid options");

            if(receivedMessage != null)
            {
                switch(receivedMessage.m_type)
                {
                    case PacketType.CHAT_MESSAGE:
                        ChatMessagePacket chatPacket = (ChatMessagePacket)receivedMessage;
                        m_clients[index].Send(new ChatMessagePacket(GetReturnMessage(chatPacket.message)));
                
                if(receivedMessage == "end")
                {
                    m_clients[index].close();
                    ConnectedClient c;
                    m_clients.TryRemove(index, out c);

                    break;
                }
                }
            }
           
        }

        private string GetReturnMessage(string code) 
        {
            switch(code)
            {
                case "0":
                    return ("Pick a number between 1 and 10");
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
