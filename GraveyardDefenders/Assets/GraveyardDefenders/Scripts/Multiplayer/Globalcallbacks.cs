//using Bolt.Matchmaking;
//using UnityEngine;
//using System;
//using Photon;

//namespace XD.Multiplayer
//{
//    [BoltGlobalBehaviour]
//    public class Globalcallbacks : Bolt.GlobalEventListener
//    {
//        public override void BoltStartBegin()
//        {
//            base.BoltStartBegin();
//            BoltNetwork.RegisterTokenClass<RoomProtocolToken>();
//            BoltNetwork.RegisterTokenClass<ServerDenyToken>();
//            BoltNetwork.RegisterTokenClass<ServerAcceptToken>();
//            BoltNetwork.RegisterTokenClass<AnimatorDataToken>(); 
//        }


//        public override void BoltStartDone()
//        {
//            base.BoltStartDone();
//            NetEvents.boltStartDone.Invoke();

//            if (BoltNetwork.IsServer) BoltMatchmaking.CreateSession($"{DateTime.Now} : {UnityEngine.Random.Range(float.NegativeInfinity, float.PositiveInfinity)}");
//            if (BoltNetwork.IsClient) BoltMatchmaking.JoinRandomSession();
//        }
//    }   
//}
