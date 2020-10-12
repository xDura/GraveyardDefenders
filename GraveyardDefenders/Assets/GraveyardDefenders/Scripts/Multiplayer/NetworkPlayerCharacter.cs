using UnityEngine;
using Photon.Pun;
using XD.Net;

namespace XD.Multiplayer
{
    public class NetworkPlayerCharacter: NetworkEntity
    {
        public PlayerCharacter pc;
        public Animator animator;

        bool last_tick_walk = false;

        protected override void OnSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (pView.IsMine)
            {
                stream.SendNext(animator.GetBool(PlayerAnimParams.Minning));
                stream.SendNext(animator.GetBool(PlayerAnimParams.ChopWood));
                stream.SendNext(animator.GetBool(PlayerAnimParams.Walk));
            }
            else
            {
                animator.SetBool(PlayerAnimParams.Minning, (bool)stream.ReceiveNext());
                animator.SetBool(PlayerAnimParams.ChopWood, (bool)stream.ReceiveNext());
                bool walk = (bool)stream.ReceiveNext();
                animator.SetBool(PlayerAnimParams.Walk, walk);
                if(last_tick_walk != walk)
                {
                    if (walk) pc.PlayWalkDust();
                    else pc.StopWalkDust();
                }
                last_tick_walk = walk;
            }
        }
    }   
}
