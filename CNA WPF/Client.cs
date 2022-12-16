using Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;


namespace CNA_WPF
{
    public class Client
    {
        private MainWindow      m_clientForm;
        private NetworkStream   m_nstream;
        private BinaryWriter    m_writer;
        private BinaryReader    m_reader;
        private TcpClient       m_tcpClient;
        private Packet          receivedPacket;
        private BinaryFormatter m_formatter;

        //encryption
        private RSACryptoServiceProvider RSAProvider;
        private RSAParameters            PublicKey;
        private RSAParameters            PrivateKey;
        private RSAParameters            ServerKey;

        // Constructor
        public Client()
        {
            m_tcpClient    = new TcpClient();

            RSAProvider = new RSACryptoServiceProvider(1024);
            PublicKey   = RSAProvider.ExportParameters(false);
            PrivateKey  = RSAProvider.ExportParameters(true);
        }

        public bool Connect(string ipAddress, int port)
        {
            try
            {
                // connects to the server
                m_tcpClient.Connect(ipAddress, port); 
                m_nstream   = m_tcpClient.GetStream();
                m_writer    = new BinaryWriter(m_nstream, Encoding.UTF8);
                m_reader    = new BinaryReader(m_nstream, Encoding.UTF8);
                m_formatter = new BinaryFormatter();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }

        }

        // Starts up the client
        public void Run()
        {
            m_clientForm = new MainWindow(this);

            m_clientForm.UpdateChatBox("You have connected with the server...");

            // new thread
            Thread thread = new Thread(ProcessServerResponse);
            thread.Start();
            m_clientForm.ShowDialog();
        }

        private void ProcessServerResponse()
        {
            try
            {
                int numberOfBytes;

                // When connected to the server
                while (m_tcpClient.Connected)
                {
                    Console.WriteLine("TCP connected");

                    // Reads the data from the server then deserializes it
                    numberOfBytes = m_reader.ReadInt32();
                    if (numberOfBytes != -1)
                    {
                        byte[] buffer = m_reader.ReadBytes(numberOfBytes);
                        MemoryStream _stream = new MemoryStream(buffer);
                        receivedPacket = m_formatter.Deserialize(_stream) as Packet;


                        if (receivedPacket != null)
                        {
                            Console.WriteLine("recieved packet");
                            switch (receivedPacket.packetType)
                            {

                                case PacketType.CHAT_MESSAGE: //Chat message
                                    {
                                        ChatMessagePacket chatPacket = (ChatMessagePacket)receivedPacket;
                                        m_clientForm.UpdateChatBox(chatPacket.message);
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Send(Packet message)
        {
            // Send chat message
            MemoryStream mStream = new MemoryStream();
            m_formatter.Serialize(mStream, message);
            byte[] buffer = mStream.GetBuffer();

            m_writer.Write(buffer.Length);
            m_writer.Write(buffer);
            m_writer.Flush();

            Console.WriteLine("message sent");
        }

        private byte[] Encrypt(byte[] data)
        {
            lock (RSAProvider)
            {
                RSAProvider.ImportParameters(ServerKey);
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
            return Encrypt(Encoding.UTF8.GetBytes(message));
        }

        private string DecryptString(byte[] message)
        {
            return Encoding.UTF8.GetString(Decrypt(message));
        }
    }
}
