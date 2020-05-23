using Bolt.Matchmaking;
using UnityEngine;
using System;

namespace XD.Multiplayer
{
    [BoltGlobalBehaviour]
    public class Globalcallbacks : Bolt.GlobalEventListener
    {
        public override void BoltStartDone()
        {
            base.BoltStartDone();
            BoltNetwork.RegisterTokenClass<AnimatorDataToken>();
            NetEvents.boltStartDone.Invoke();

            if (BoltNetwork.IsServer) BoltMatchmaking.CreateSession($"{DateTime.Now} : {UnityEngine.Random.Range(float.NegativeInfinity, float.PositiveInfinity)}");
            if (BoltNetwork.IsClient) BoltMatchmaking.JoinRandomSession();
        }
    }   
}
