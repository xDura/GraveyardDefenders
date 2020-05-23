using UnityEngine;
using Bolt;
using UdpKit;

namespace XD.Multiplayer
{
    public class AnimatorDataToken : IProtocolToken
    {
        public float normalizedTime;
        public int stateNameHash;

        public AnimatorDataToken() {}

        public AnimatorDataToken(float a_normalizedTime, int a_stateNameHash)
        {
            normalizedTime = a_normalizedTime;
            stateNameHash = a_stateNameHash;
        }

        public void Read(UdpPacket packet)
        {
            normalizedTime = packet.ReadFloat();
            stateNameHash = packet.ReadInt();
        }

        public void Write(UdpPacket packet)
        {
            packet.WriteFloat(normalizedTime);
            packet.WriteInt(stateNameHash);
        }

        public bool TokenEquals(AnimatorDataToken token2)
        {
            return normalizedTime == token2.normalizedTime && stateNameHash == token2.stateNameHash;
        }
    }
}
