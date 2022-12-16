using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Packets;
using System.Security.Cryptography;
using System.Threading.Tasks.Dataflow;

namespace ServerProj
{
    internal class ConnectedClient
    {
        private Socket          m_socket;
        private NetworkStream   m_nstream;
        private BinaryReader    m_reader;
        private BinaryWriter    m_writer;
        private BinaryFormatter m_formatter;
        private object          m_readlock;
        private object          m_writelock;

        private RSACryptoServiceProvider RSAProvider;
        private RSAParameters            PublicKey;
        private RSAParameters            PrivateKey;
        private RSAParameters            ClientKey;

        // Constructor
        public ConnectedClient(Socket _socket)
        {
            m_writelock   = new object();
            m_readlock    = new object();
            m_socket      = _socket;

            m_nstream     = new NetworkStream(m_socket);
            m_reader      = new BinaryReader(m_nstream, Encoding.UTF8);
            m_writer      = new BinaryWriter(m_nstream, Encoding.UTF8);
            m_formatter   = new BinaryFormatter();

            RSAProvider = new RSACryptoServiceProvider(1024);
            PublicKey   = RSAProvider.ExportParameters(false);
            PrivateKey  = RSAProvider.ExportParameters(true);
        }
        public void close()
        {
            m_nstream.Close();
            m_reader.Close();
            m_writer.Close();
            m_socket.Close();
        }
        public Packet Read()
        {
            // Only one thread can access
            lock (m_readlock)
            {
                int numberOfBytes = -1;
                try
                {
                    if ((numberOfBytes = m_reader.ReadInt32()) != -1)
                    {
                        byte[] buffer = m_reader.ReadBytes(numberOfBytes);

                        MemoryStream memoryStream = new MemoryStream(buffer);

                        Packet packet = m_formatter.Deserialize(memoryStream) as Packet;
                        return packet;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception " + e.Message);
                }
                    return null;
            }
        }

        // Serializes then sends to the client
        public void Send(Packet message)
        {
            lock (m_writelock)
            {
                MemoryStream ms = new MemoryStream(); // Stores binary data
                m_formatter.Serialize(ms, message);
                byte[] buffer = ms.GetBuffer();

                m_writer.Write(buffer.Length);
                m_writer.Write(buffer);
                m_writer.Flush();
            }
        }

        private byte[] Encrypt(byte[] data)
        {
            lock (RSAProvider)
            {
                RSAProvider.ImportParameters(ClientKey);
                return RSAProvider.Encrypt(data, true);
            }
        }

        private byte[] Decrypt(byte[] data)
        {
            lock (RSAProvider)
            {
                RSAProvider.ImportParameters(PrivateKey);
                return RSAProvider.Decrypt(data, true);
            }
        }

        private byte[] EncryptString(string message)
        {
           return Encrypt( Encoding.UTF8.GetBytes(message));
        }

        private string DecryptString(byte[] message)
        {
            return Encoding.UTF8.GetString(Decrypt(message));
        }
    }
}
