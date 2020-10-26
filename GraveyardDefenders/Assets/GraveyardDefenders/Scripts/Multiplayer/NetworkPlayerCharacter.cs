using UnityEngine;
using Photon.Pun;
using XD.Net;
using Photon.Realtime;

namespace XD.Multiplayer
{
    public class NetworkPlayerCharacter: NetworkEntity
    {
        public PlayerCharacter pc;
        public Animator animator;

        bool last_tick_walk = false;

        void TransferOwnerShip()
        {
            pView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        protected override void OnAttachedOwner()
        {
            base.OnAttachedOwner();
        }

        public override void OnAtachedNonOwner(Player player)
        {
            base.OnAtachedNonOwner(player);
            if (NetManager.Instance.IsMaster)
            {
                pView.TransferOwnership(PhotonNetwork.LocalPlayer);
                pView.TransferOwnership(player);
            }
            pc.enabled = false;
            pc.isLocal = false;
            pc.interactSystem.enabled = false;
        }

        protected override void OnSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
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
