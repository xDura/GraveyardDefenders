using UdpKit;
using System;

namespace Photon
{
    public class RoomProtocolToken : Bolt.IProtocolToken
    {
        public String ArbitraryData;
        public String password;

        public void Read(UdpPacket packet)
        {
            ArbitraryData = packet.ReadString();
            password = packet.ReadString();
        }

        public void Write(UdpPacket packet)
        {
            packet.WriteString(ArbitraryData);
            packet.WriteString(password);
        }
    }

    public class ServerAcceptToken : Bolt.IProtocolToken
    {
        public String data;

        public void Read(UdpPacket packet)
        {
            data = packet.ReadString();
        }

        public void Write(UdpPacket packet)
        {
            packet.WriteString(data);
        }
    }

    public enum SERVER_DENY_REASONS
    {
        UNKNOWN = 0,
        //can't let the user enter because
        //it breaks platform restriction like: PS4 - XBOX
        PLATFORM_MISMATCH = 1,
    }

    public class ServerDenyToken : Bolt.IProtocolToken
    {
        public SERVER_DENY_REASONS reason;
        public ServerDenyToken() { }
        public ServerDenyToken(SERVER_DENY_REASONS a_reason)
        {
            reason = a_reason;
        }

        public void Read(UdpPacket packet)
        {
            reason = (SERVER_DENY_REASONS)packet.ReadInt(10);
        }

        public void Write(UdpPacket packet)
        {
            packet.WriteInt((int)reason, 10);
        }
    }
}