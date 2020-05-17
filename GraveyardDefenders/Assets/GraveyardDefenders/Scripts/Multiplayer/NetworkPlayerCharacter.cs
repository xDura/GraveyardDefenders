using UnityEngine;
using Bolt;
using System;
using System.Reflection;

namespace XD.Multiplayer
{
    public class NetworkPlayerCharacter : EntityBehaviour<IPlayerCharacter>
    {
        public Animator animator;

        AnimatorDataToken token = new AnimatorDataToken();
        AnimatorDataToken lastToken = new AnimatorDataToken();

        public void Awake()
        {
            NetEvents.boltStartDone.AddListener(OnBoltStartDone);
        }

        public void OnBoltStartDone()
        {
            if (BoltNetwork.IsServer) 
                BoltNetwork.Attach(entity);
        }

        public override void Attached()
        {
            base.Attached();
            state.SetTransforms(state.Transform, transform);
            if (entity.IsOwner)
            {

            }
            else
            {
                animator.enabled = false;
            }
        }

        public override void Detached()
        {
            base.Detached();
        }

        public override void SimulateOwner()
        {
            base.SimulateOwner();
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            token.stateNameHash = info.fullPathHash;
            token.normalizedTime = info.normalizedTime;
            if (token.normalizedTime > 1.0f) token.normalizedTime %= 1.0f;
            if (!token.TokenEquals(lastToken))
            {
                state.AnimatorStatesData = token;
                lastToken.normalizedTime = token.normalizedTime;
                lastToken.stateNameHash = token.stateNameHash;
            }
        }

        public void Update()
        {
            if (!(BoltNetwork.IsRunning && BoltNetwork.IsConnected)) return;
            if (!entity.IsAttached) return;
            if (entity.IsOwner) return;

            token = state.AnimatorStatesData as AnimatorDataToken;
            if (token == null) { }
            else
            {
                AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
                if(info.fullPathHash != token.stateNameHash)
                {
                    Debug.Log("Was In different state");
                    animator.Play(token.stateNameHash, 0, token.normalizedTime);
                }
            }

            animator.Update(Time.deltaTime);
        }
    }   
}
