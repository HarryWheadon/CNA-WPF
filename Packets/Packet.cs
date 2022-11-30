using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Packets
{
        public enum PacketType
        {
            CHAT_MESSAGE,
            PRIVATE_MESSAGE,
            CLIENT_NAME,
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
        }
    }
}