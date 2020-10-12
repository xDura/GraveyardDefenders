using UnityEngine;
using Photon.Pun;
using XD.Net;
using Photon.Realtime;

namespace XD
{
    public class NetworkEntity : MonoBehaviourPunCallbacks, IPunObservable, IPunOwnershipCallbacks
    {
        [Header("Assinables")]
        public PhotonView pView;

        [Header("Variables")]
        public PREFAB_ID prefab_id;
        public bool autoattach = true;
        public bool isSceneEntity = false;

        public bool IsRemote => !pView.IsMine && Attached;
        public bool Attached => pView.ViewID != 0;

        #region VIRTUAL_API
        protected virtual void OnAwake() {}
        protected virtual void OnStart() 
        {
            if (!IsRemote && autoattach)
            {
                if (NetManager.Instance.IsMaster) AttachToNetwork(); 
                else if (NetManager.Instance.IsClient)
                {
                    if (isSceneEntity) DestroySceneEntityClient();
                    else AttachToNetwork();
                } 
            }
        }
        protected virtual void OnUpdate() {}
        protected virtual void UpdateOwner() {}
        protected virtual void UpdateNonOwner() {}
        protected virtual void OnAttachedOwner() { DontDestroyOnLoad(gameObject); }
        public virtual void OnAtachedNonOwner(Player player) { DontDestroyOnLoad(gameObject); }
        protected virtual void AttachToNetwork() 
        { 
            NetManager.Instance.AttachPhotonView(transform, pView, prefab_id, isSceneEntity); 
            OnAttachedOwner(); 
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { OnSerializeView(stream, info); }
        protected virtual void OnSerializeView(PhotonStream stream, PhotonMessageInfo info) {}
        public override void OnJoinedRoom() 
        {
            base.OnJoinedRoom();
            JoinedRoom();
        }

        public override void OnCreatedRoom()
        { 
            base.OnCreatedRoom();
            CreatedRoom();
        }

        protected virtual void JoinedRoom() 
        {
            if(NetManager.Instance.IsClient && autoattach)
            {
                if (isSceneEntity) DestroySceneEntityClient();
                else if(!Attached) AttachToNetwork();
            }
        }

        protected virtual void CreatedRoom() 
        {
            if (!Attached && autoattach) AttachToNetwork();
        }
        #endregion

        #region UNITY_CALLBACKS
        private void Awake() { OnAwake(); }
        private void Start() { OnStart(); }

        private void Update()
        {
            OnUpdate();
            if (NetManager.Instance.IsMulti)
            {
                if (pView.IsMine) UpdateOwner();
                else UpdateNonOwner();
            }
        }
        #endregion

        public void DestroySceneEntityClient()
        {
            if (isSceneEntity)
            {
                Debug.LogError($"Client had local scene entity {name}");
                Destroy(this.gameObject);
            }
        }

        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer){}
        public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner){}
    }
}
