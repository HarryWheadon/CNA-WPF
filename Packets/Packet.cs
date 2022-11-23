using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Packets
{
    public class Packet
    {
        public Enum PacketType
        {
            Chat Message,
            Private Message,
            Client Name,
        }
        public PacketType packetType()
        {

        }
    }

    public class ChatMessagePacket : Packet
    {
        public string message;

        public ChatMessagePacket(string message)
        {
            this.message = message;
        }
    }
}