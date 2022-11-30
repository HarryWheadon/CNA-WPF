using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Packets;

namespace ServerProj
{
    internal class ConnectedClient
    {
        private Socket          m_socket;
        private NetworkStream   m_stream;
        private BinaryReader    m_reader;
        private BinaryWriter    m_writer;
        private BinaryFormatter m_formatter;
        private object          m_readlock;
        private object          m_writelock;

        public ConnectedClient(Socket socket)
        {
            m_writelock = new object();
            m_readlock  = new object();
            m_socket    = socket;

            m_stream    = new NetworkStream(socket, true);
            m_reader    = new BinaryReader(m_stream, Encoding.UTF8);
            m_writer    = new BinaryWriter(m_stream, Encoding.UTF8);
            m_formatter = new BinaryFormatter();
        }
        public void close()
        {
            m_stream.Close();
            m_reader.Close();
            m_writer.Close();
            m_socket.Close();
        }
        public Packet Read()
        {
            lock (m_readlock)
            {
                int numberOfBytes = m_reader.ReadInt32();
                if (numberOfBytes != -1)
                {
                    byte[] buffer = m_reader.ReadBytes(numberOfBytes);
                    MemoryStream ms = new MemoryStream(buffer);
                    return m_formatter.Deserialize(m_stream) as Packet;
                }
                else
                    return null;
            }
        }
        public void Send(Packet Message)
        {
            lock (m_writelock)
            {
                MemoryStream ms = new MemoryStream();
                m_formatter.Serialize(m_stream, Message);
                byte[] buffer = ms.GetBuffer();
                m_writer.Write(buffer.Length);
                m_writer.Flush();
            }
        }
    }
}
