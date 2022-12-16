using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography;

namespace Packets
{
        // Enum to store types of packets
        public enum PacketType
        {
            CHAT_MESSAGE,
            PRIVATE_MESSAGE,
            CLIENT_NAME,
            ENCRYPT_MESSAGE,
        }

        [Serializable]
        public class Packet
        {
        public PacketType packetType { get; set; }
        }

    [Serializable]
    public class ChatMessagePacket : Packet
    {
        public string message;

        public ChatMessagePacket(string message)
        {
            this.message = message;
            packetType = PacketType.CHAT_MESSAGE;
        }
    }

    [Serializable]
    public class EncryptPacket : Packet
    {
        public RSAParameters PublicKey;
        public EncryptPacket(RSAParameters PublicKey)
        {
            this.PublicKey = PublicKey;
            packetType = PacketType.ENCRYPT_MESSAGE;
        }
    }
}