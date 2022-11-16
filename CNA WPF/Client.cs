using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CNA_WPF
{
    internal class Client
    {
        private NetworkStream m_stream;
        private StreamWriter m_writer;
        private StreamReader m_reader;
        private TcpClient m_tcpClient;

        public Client()
        {
            m_tcpClient = new TcpClient(); 
        }

        public bool Connect(string ipAddress, int port)
        {
            try
            {
                m_tcpClient.Connect(ipAddress, port);
                m_stream = m_tcpClient.GetStream();
                m_writer = new StreamWriter(m_stream, Encoding.UTF8);
                m_reader = new StreamReader(m_stream, Encoding.UTF8);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }

        }

        public void Run()
        {
            string userInput;
            ProcessServerResponse();
            while((userInput = Console.ReadLine()) != null)
            {
                m_writer.WriteLine(userInput);
                m_writer.Flush();

                ProcessServerResponse();

                if(userInput == ("end"))
                {
                    break;
                }
            }
            m_tcpClient.Close();
        }

        private void ProcessServerResponse()
        {
            Console.WriteLine("Server says: " + m_reader.ReadLine());
        }
    }
}
