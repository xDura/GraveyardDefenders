using UnityEngine;
//using Bolt;

namespace XD.Multiplayer
{
    public class NetworkPlayerCharacter : MonoBehaviour
    {
        public PlayerCharacter pc;
        public Animator animator;

        //public void Awake()
        //{
        //    NetEvents.boltStartDone.AddListener(OnBoltStartDone);
        //}

        //public void OnBoltStartDone()
        //{
        //    BoltNetwork.Attach(entity);
        //}

        //public override void Attached()
        //{
        //    base.Attached();
        //    state.SetTransforms(state.Transform, transform);
        //    if (entity.IsOwner){}
        //    else
        //    {
        //        pc.isLocal = false;
        //        pc.enabled = false;
        //        state.AddCallback(PlayerAnimParams.Minning, OnMinningChanged);
        //        state.AddCallback(PlayerAnimParams.ChopWood, OnChopWoodChanged);
        //        state.AddCallback(PlayerAnimParams.Walk, OnWalkChanged);
        //    }
        //}

        //#region VARIABLE_CALLBACKS
        //void OnMinningChanged() { animator.SetBool(PlayerAnimParams.Minning, state.Minning); }
        //void OnChopWoodChanged() { animator.SetBool(PlayerAnimParams.ChopWood, state.ChopWood); }
        //void OnWalkChanged() 
        //{
        //    bool walk = state.Walk;
        //    animator.SetBool(PlayerAnimParams.Walk, walk);
        //}
        //#endregion

        //public override void Detached()
        //{
        //    base.Detached();
        //}

        //public override void SimulateOwner()
        //{
        //    base.SimulateOwner();
        //    state.Minning = animator.GetBool(PlayerAnimParams.Minning);
        //    state.ChopWood = animator.GetBool(PlayerAnimParams.ChopWood);
        //    state.Walk = animator.GetBool(PlayerAnimParams.Walk);
        //}

        //void Update()
        //{
        //    if (!(BoltNetwork.IsRunning && BoltNetwork.IsConnected)) return;
        //    if (!entity.IsAttached) return;
        //    if (entity.IsOwner) UpdateOwner();
        //    else UpdateNonOwner();
        //}

        //void UpdateOwner() {}
        //void UpdateNonOwner() 
        //{
        //    if (state.Walk) pc.PlayWalkDust();
        //    else pc.StopWalkDust();
        //}
    }   
}
