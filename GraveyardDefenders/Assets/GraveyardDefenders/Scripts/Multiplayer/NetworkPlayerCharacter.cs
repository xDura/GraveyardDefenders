using UnityEngine;
using Bolt;

namespace XD.Multiplayer
{
    public class NetworkPlayerCharacter : EntityBehaviour<IPlayerCharacter>
    {
        public PlayerCharacter pc;
        public Animator animator;

        public void Awake()
        {
            NetEvents.boltStartDone.AddListener(OnBoltStartDone);
        }

        public void OnBoltStartDone()
        {
            BoltNetwork.Attach(entity);
        }

        public override void Attached()
        {
            base.Attached();
            state.SetTransforms(state.Transform, transform);
            if (entity.IsOwner){}
            else
            {
                pc.isLocal = false;
                state.AddCallback("Minning", OnMinningChanged);
                state.AddCallback("ChopWood", OnChopWoodChanged);
                state.AddCallback("Walk", OnWalkChanged);
            }
        }

        #region VARIABLE_CALLBACKS
        void OnMinningChanged() { animator.SetBool("Minning", state.Minning); }
        void OnChopWoodChanged() { animator.SetBool("ChopWood", state.ChopWood); }
        void OnWalkChanged() 
        {
            bool walk = state.Walk;
            animator.SetBool("Walk", walk);
            if (walk) pc.PlayWalkDust();
            else pc.StopWalkDust();
        }
        #endregion

        public override void Detached()
        {
            base.Detached();
        }

        public override void SimulateOwner()
        {
            base.SimulateOwner();
            state.Minning = animator.GetBool("Minning");
            state.ChopWood = animator.GetBool("ChopWood");
            state.Walk = animator.GetBool("Walk");
        }

        void Update()
        {
            if (!(BoltNetwork.IsRunning && BoltNetwork.IsConnected)) return;
            if (!entity.IsAttached) return;
            if (entity.IsOwner) UpdateOwner();
            else UpdateNonOwner();
        }

        void UpdateOwner() {}
        void UpdateNonOwner() {}
    }   
}
