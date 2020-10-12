using UnityEngine;
using Photon.Pun;
using XD.Net;

namespace XD.Multiplayer
{
    public class NetworkPlayerCharacter: MonoBehaviourPunCallbacks, IPunObservable
    {
        public PlayerCharacter pc;
        public Animator animator;
        public PhotonView pView;

        bool last_tick_walk = false;

        private void Awake()
        {
            if (NetManager.Instance.InRoom) AttachToNetwork();
            else NetEvents.OnCreatedRoom.AddListener(AttachToNetwork);
        }

        void AttachToNetwork()
        {
            NetManager.Instance.AttachPhotonView(transform, pView, 1);
            DontDestroyOnLoad(this);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
