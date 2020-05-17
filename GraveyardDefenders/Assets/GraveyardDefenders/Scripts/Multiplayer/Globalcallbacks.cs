using UnityEngine;

namespace XD.Multiplayer
{
    [BoltGlobalBehaviour]
    public class Globalcallbacks : Bolt.GlobalEventListener
    {
        public override void BoltStartDone()
        {
            base.BoltStartDone();
            NetEvents.boltStartDone.Invoke();
            BoltNetwork.RegisterTokenClass<AnimatorDataToken>();
        }
    }   
}
