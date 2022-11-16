using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ServerProj
{
    internal class ConnectedClient
    {
        private Socket m_socket;
        private NetworkStream m_stream;
        private StreamReader m_reader;
        private StreamWriter m_writer;
        private object m_readlock;
        private object m_writelock;

        public ConnectedClient(Socket socket)
        {
            m_writelock = new object();
            m_readlock = new object();
            m_socket = socket;

             m_stream = new NetworkStream(socket, true);
             m_reader = new StreamReader(m_stream, Encoding.UTF8);
             m_writer = new StreamWriter(m_stream, Encoding.UTF8);
        }
        public void close()
        {
            m_stream.Close();
            m_reader.Close();
            m_writer.Close();
            m_socket.Close();
        }
        public string Read()
        {
            lock(m_readlock)
            {
                return m_reader.ReadLine();
            }
        }
        public void Send(string Message)
        {
            lock (m_writelock)
            {
                m_writer.WriteLine(Message);
                m_writer.Flush();
            }
        }
    }
}
